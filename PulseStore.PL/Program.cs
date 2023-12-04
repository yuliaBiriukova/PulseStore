using Azure.Storage.Blobs;
using PulseStore.BLL.Repositories;
using PulseStore.BLL.Services.BlobStorage;
using PulseStore.BLL.Services.Category;
using PulseStore.BLL.Services.Photo;
using PulseStore.BLL.Services.Products;
using PulseStore.BLL.Services.SearchHistory;
using PulseStore.BLL.Services.UserCartProducts;
using PulseStore.DAL.Database;
using PulseStore.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using PulseStore.BLL.Services.RecentlyViewedProduct;
using PulseStore.BLL.Services.Orders;
using PulseStore.BLL.Services.Stock;
using PulseStore.BLL.Entities;
using FluentValidation;
using PulseStore.BLL.Behaviors;
using MediatR;
using PulseStore.PL.Extensions;
using PulseStore.BLL.Repositories.Security;
using PulseStore.DAL.Repositories.Security;
using PulseStore.BLL.Services.TemplateFile;
using PulseStore.BLL.ExternalServices.EmailSender;
using Microsoft.AspNetCore.Http.Features;
using PulseStore.PL.ViewModels.Payment;
using Stripe;
using PulseStore.BLL.Services.TemplateToHtml;
using PulseStore.BLL.Services.OrderDocument;
using PulseStore.BLL.Models.Email;
using PulseStore.BLL.Services.OrderLetters;
using PulseStore.BLL.Helpers.HtmlToPdf;
using PulseStore.BLL.Services.StockProducts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PulseStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PulseStoreConnection"), b => b.MigrationsAssembly("PulseStore.DAL")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();
builder.Services.AddScoped<IUserCartProductRepository, UserCartProductRepository>();
builder.Services.AddScoped<IStockProductRepository, StockProductRepository>();
builder.Services.AddScoped<IUserProductViewRepository, UserProductViewRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<INfcDeviceRepository, NfcDeviceRepository>();
builder.Services.AddScoped<ISecurityUserRepository, SecurityUserRepository>();
builder.Services.AddScoped<ITemplateFileRepository, TemplateFileRepository>();
builder.Services.AddScoped<IOrderDocumentRepository, OrderDocumentRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, PulseStore.BLL.Services.Products.ProductService>();
builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ISearchHistoryService, SearchHistoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IRecentlyViewedProductService, RecentlyViewedProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITemplateFileService, TemplateFileService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IStockProductService, StockProductService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<ITemplateToHtmlService, TemplateToHtmlService>();
builder.Services.AddScoped<IOrderDocumentService, OrderDocumentService>();
builder.Services.AddScoped<IOrderLetterService, OrderLetterService>();

builder.Services.AddScoped<BlobServiceClient>(_ => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorageConnection")));

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

builder.Services.Configure<FormOptions>(o => {
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PulseStore API",
        Version = "v1",
        Description = "APIs for PulseStore",
    });

    // Define the security scheme for Swagger (if needed)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    // Include the security requirement for JWT authentication (if needed)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            new string[] { }
        },
    });

    // used for custom controllers grouping
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[]
            {
                api.GroupName
            };
        }

        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            return new[]
            {
                controllerActionDescriptor.ControllerName
            };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    // used for custom controllers grouping
    c.DocInclusionPredicate((name, api) => true);
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServer:Host"]; // The issuer URL
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = "InternBack"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthorizedUser", policy =>
    {
        policy.RequireRole("admin", "customer");
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    });

    options.AddPolicy("RequireAdminRole", policy =>
    {
        policy.RequireRole("admin");
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddHttpClient();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(PulseStore.BLL.Entities.Product).Assembly);
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(typeof(PulseStore.BLL.Entities.Product).Assembly);

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PulseStore API V1");
        c.RoutePrefix = "swagger";
    });
}*/

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PulseStore API V1");
    c.RoutePrefix = "swagger";
});

var customerHost = builder.Configuration["Customer:Host"];
var adminHost = builder.Configuration["Admin:Host"];

if (customerHost is null || adminHost is null)
{
    throw new ConfigurationErrorsException("CORS configuration error. Client host names are missing in configurations.");
} 
else
{
    app.UseCors(builder =>
       builder
           .WithOrigins(customerHost, adminHost)
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
    );
}

app.UseHttpsRedirection();

app.UseTokenValidation();

app.UseAuthentication();
app.UseAuthorization();

app.UseFluentValidationExceptionHandler();

app.MapControllers();

var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
var blob = service.GetRequiredService<IBlobStorageService>();
try
{
    await blob.CheckContainer();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during creation or check of Container");
}

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.Run();
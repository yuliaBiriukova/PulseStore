using SelectPdf;

namespace PulseStore.BLL.Helpers.HtmlToPdf;

public static class HtmlToPdfConverter
{
    /// <summary>
    ///     Converts HTML stream to PDF stream.
    /// </summary>
    /// <param name="htmlStream"><see cref="Stream"/> with HTML data.</param>
    /// <returns>
    ///     <see cref="Stream"/> with PDF.
    /// </returns>
    public static Stream ConvertHtmlToPdf(Stream htmlStream)
    {
        using (var streamReader = new StreamReader(htmlStream))
        {
            var htmlContent = streamReader.ReadToEnd();

            var converter = new SelectPdf.HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MarginLeft = 30;
            converter.Options.MarginRight = 30;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 30;

            var pdfDoc = converter.ConvertHtmlString(htmlContent);

            var pdfStream = new MemoryStream();
            pdfDoc.Save(pdfStream);
            pdfDoc.Close();
            pdfStream.Seek(0, SeekOrigin.Begin);
            return pdfStream;
        }
    }
}
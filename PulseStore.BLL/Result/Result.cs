namespace PulseStore.BLL.Result;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? value;
    private readonly TError? error;

    public bool IsError { get; }

    public TValue Value => value!;

    public TError Error => error!;

    private Result(TValue value)
    {
        IsError = false;
        this.value = value;
        this.error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        this.value = default;
        this.error = error;
    }
    

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);
    
    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) =>
        !IsError ? success(Value) : failure(Error);
}
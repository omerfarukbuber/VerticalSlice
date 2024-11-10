namespace Newsletter.API.Shared;

public class Result<T>
{
    protected internal Result(T data, bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
        Value = data;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public T Value { get;}

    public static Result<T> Success(T data)
    {
        return new Result<T>(data, true, Error.None);
    }

    public static Result<T?> Failure(Error error)
    {
        return new Result<T?>(default, false, error);
    }
}
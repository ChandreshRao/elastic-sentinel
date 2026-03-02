namespace ElasticSentinel.Application.Common.Models;

/// <summary>
/// Represents the result of an operation that can either succeed with a value or fail with an error
/// </summary>
/// <typeparam name="TValue">The type of the success value</typeparam>
public class Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    /// <summary>
    /// Indicates whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the value if successful, otherwise throws
    /// </summary>
    public TValue Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot access Value of a failed result");
            }
            return _value!;
        }
    }

    /// <summary>
    /// Gets the error if failed, otherwise throws
    /// </summary>
    public Error Error
    {
        get
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Cannot access Error of a successful result");
            }
            return _error!;
        }
    }

    private Result(TValue value)
    {
        _value = value;
        _error = null;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        _value = default;
        _error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Creates a failed result with an error
    /// </summary>
    public static Result<TValue> Failure(Error error) => new(error);

    /// <summary>
    /// Implicit conversion from value to Result
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    /// <summary>
    /// Implicit conversion from Error to Result
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => Failure(error);

    /// <summary>
    /// Pattern matching support
    /// </summary>
    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_value!) : onFailure(_error!);
    }
}

/// <summary>
/// Represents the result of an operation that can succeed or fail without a return value
/// </summary>
public class Result
{
    private readonly Error? _error;

    /// <summary>
    /// Indicates whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation failed
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error if failed, otherwise throws
    /// </summary>
    public Error Error
    {
        get
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Cannot access Error of a successful result");
            }
            return _error!;
        }
    }

    private Result(bool isSuccess, Error? error = null)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static Result Success() => new(true);

    /// <summary>
    /// Creates a failed result with an error
    /// </summary>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Implicit conversion from Error to Result
    /// </summary>
    public static implicit operator Result(Error error) => Failure(error);

    /// <summary>
    /// Pattern matching support
    /// </summary>
    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(_error!);
    }
}

using Skuzzle.Core.Lib.ResultClass.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Skuzzle.Core.Lib.ResultClass;

// TODO: Look at switching to FluentResult library /nb
public class Result
{
    public bool IsSuccess { get; private set; }

    public bool IsFailure => !IsSuccess;

    public string ErrorMessage { get; private set; }

    private Exception? _exception;

    public Exception? Exception
    {
        get
        {
            Contract.Requires(IsSuccess);
            return _exception;
        }

        [param: AllowNull]
        private set { _exception = value; }
    }

    protected Result(bool success, string errorMessage)
    {
        Contract.Requires(success || !string.IsNullOrEmpty(errorMessage));
        Contract.Requires(!success || string.IsNullOrEmpty(errorMessage));

        IsSuccess = success;
        ErrorMessage = errorMessage;
    }

    protected Result(Exception? exception, bool success, string errorMessage)
    {
        Contract.Requires(success || !string.IsNullOrEmpty(errorMessage));
        Contract.Requires(!success || string.IsNullOrEmpty(errorMessage));
        Contract.Requires(success || exception == null);

        IsSuccess = success;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    public static Result Fail(string errorMessage) =>
        new (false, errorMessage);

    public static Result Fail(string errorMessage, params object?[] args) =>
        new (false, string.Format(errorMessage.ReformatPlaceholders(), args));

    public static Result Fail(Exception exception, string errorMessage, params object?[] args) =>
        new (exception, false, string.Format(errorMessage.ReformatPlaceholders(), args));

    public static Result<T> Fail<T>(string errorMessage) =>
        new(default, false, errorMessage);

    public static Result<T> Fail<T>(string errorMessage, params object?[] args) =>
        new(default, false, string.Format(errorMessage.ReformatPlaceholders(), args));

    public static Result<T> Fail<T>(Exception exception, string errorMessage, params object?[] args) =>
        new(default, exception, false, string.Format(errorMessage.ReformatPlaceholders(), args));

    public static Result Ok() =>
        new (true, string.Empty);

    public static Result<T> Ok<T>(T value) =>
        new(value, true, string.Empty);

    public static Result Combine(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Ok();
    }
}

public class Result<T> : Result
{
    private T? _value;

    public T? Value
    {
        get
        {
            Contract.Requires(IsSuccess);

            return _value;
        }

        [param: AllowNull]
        private set { _value = value; }
    }

    protected internal Result([AllowNull] T value, bool success, string errorMessage)
        : base(success, errorMessage)
    {
        Contract.Requires(value != null || !success);

        Value = value;
    }

    protected internal Result([AllowNull] T value, Exception exception, bool success, string errorMessage)
    : base(exception, success, errorMessage)
    {
        Contract.Requires(value != null || !success);

        Value = value;
    }
}
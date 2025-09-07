using System.Net;

namespace ECP.Shared
{
    //Non-generic result for void operations
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public HttpStatusCode? StatusCode { get; }


        protected Result(bool isSuccess, string? errorMessage, HttpStatusCode? httpStatus = null)
        {
            IsSuccess = isSuccess;
            Message = errorMessage;
            StatusCode = httpStatus;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string errorMessage, HttpStatusCode? httpStatus = null) => new Result(false, errorMessage, httpStatus);
    }

    //result for all other return types
    public class Result<T> : Result
    {
        public T? Value { get; }

        protected Result(T? value, bool isSuccess, string? errorMessage, HttpStatusCode? httpStatus)
            : base(isSuccess, errorMessage, httpStatus)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value, true, null, null);
        public static Result<T> Failure(string errorMessage, HttpStatusCode? httpStatus = null) => new Result<T>(default, false, errorMessage, httpStatus);
    }
}

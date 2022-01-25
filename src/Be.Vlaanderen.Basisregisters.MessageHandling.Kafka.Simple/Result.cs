namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Simple
{
    public class Result
    {
        private protected Result()
        { }

        public bool IsSuccess { get; init; }
        public string? Error { get; init; }
        public string? ErrorReason { get; init; }

        public static Result Success() => new Result { IsSuccess = true };
        public static Result Failure(string error, string errorReason) => new Result { IsSuccess = false, Error = error, ErrorReason = errorReason };
    }

    public class Result<T> : Result
    {
        private Result()
        { }

        public T? Message { get; init; }

        public static Result<T> Success(T? message) => new Result<T> { IsSuccess = true, Message = message };
        public new static Result<T> Failure(string error, string errorReason) => new Result<T> { IsSuccess = false, Error = error, ErrorReason = errorReason };
    }
}

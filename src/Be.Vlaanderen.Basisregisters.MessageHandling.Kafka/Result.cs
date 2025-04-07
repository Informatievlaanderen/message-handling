namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka
{
    public sealed class Result
    {
        public bool IsSuccess { get; init; }
        public string? Error { get; init; }
        public string? ErrorReason { get; init; }

        private Result()
        { }

        public Offset? Offset { get; init; }

        public static Result Success(Offset offset) => new Result { IsSuccess = true, Offset = offset };
        public static Result Failure(string error, string errorReason) => new Result { IsSuccess = false, Error = error, ErrorReason = errorReason };
    }
}

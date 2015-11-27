namespace Sources
{
    public class Result<T> : Result
    {
        protected internal Result(T value, bool success, string error)
            : base(success, error)
        {
            this.Value = value;
        }

        public T Value { get; set; }
    }

    public class Result
    {
        protected Result(bool success, string error)
        {
            this.Success = success;
            this.Error = error;
        }

        public string Error { get; private set; }

        public bool Failure => !this.Success;

        public bool Success { get; }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
    }
}
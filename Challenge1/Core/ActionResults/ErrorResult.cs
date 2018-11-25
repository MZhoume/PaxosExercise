namespace Challenge1.Core.ActionResults
{
    public class ErrorResult
    {
        public ErrorResult(string message)
        {
            this.Message = message;
        }

        public ErrorResult(string message, object errors)
            : this(message)
        {
            this.Errors = errors;
        }

        public object Errors { get; set; }

        public string Message { get; set; }
    }
}

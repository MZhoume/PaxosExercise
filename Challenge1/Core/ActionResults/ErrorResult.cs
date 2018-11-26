namespace Challenge1.Core.ActionResults
{
    /// <summary>
    /// <see cref="ErrorResult"/> provides a way to construct a full-bodied error result with a message
    /// and an optional error list.
    /// </summary>
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

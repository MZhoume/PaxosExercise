using System;

namespace Challenge1.Features.Models
{
    /// <summary>
    /// Response model for endpoint /messages/{hash}.
    /// </summary>
    public class MessageResponseModel
    {
        public MessageResponseModel()
        {
        }

        public MessageResponseModel(string message)
        {
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Message { get; internal set; }
    }
}

using FluentValidation;

namespace Challenge1.Features.Models
{
    /// <summary>
    /// Request model for endpoint /messages.
    /// </summary>
    public class MessageRequestModel
    {
        public MessageRequestModel()
        {
        }

        public MessageRequestModel(string message)
        {
            this.Message = message;
        }

        public string Message { internal get; set; }

        public string Digest { get; internal set; }
    }

    public class MessageRequestModelValidator : AbstractValidator<MessageRequestModel>
    {
        public MessageRequestModelValidator()
        {
            this.RuleFor(r => r.Message).NotEmpty();
        }
    }
}

using FluentValidation;

namespace Challenge1.Features.Models
{
    public class MessageRequestModel
    {
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

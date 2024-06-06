using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using FluentValidation.Results;
using FluentValidation;
using Bridgenext.DataAccess.Interfaces;

namespace Bridgenext.Engine.Validators
{
    public  class CreateDocumentRequestValidator : AbstractValidator<CreateDocumentRequest>
    {
        public CreateDocumentRequestValidator(IUserRepository userRepository)
        {

            RuleFor(x => x.Name).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(DocumentExceptions.RequiredName);

            RuleFor(x => x.Description).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(DocumentExceptions.RequiredDescription);

            RuleFor(x => new {x.File ,x.Content}).Must(y => !string.IsNullOrEmpty(y.Content) || !string.IsNullOrEmpty(y.File))
                .WithMessage(DocumentExceptions.RequiredFileContentNotNull);

           /* RuleFor(x => new { x.File, x.Content }).Must(y => string.IsNullOrEmpty(y.Content) && string.IsNullOrEmpty(y.File))
                .WithMessage(DocumentExceptions.RequiredFileContentSameTime);*/

            RuleFor(x => x.CreateUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(DocumentExceptions.CreateAddressNotExist);

            RuleFor(x => x.CreateUser).Must(y => userRepository.IdExistsAsync(y).Result)
                .When(z => !string.IsNullOrEmpty(z.CreateUser))
                .WithMessage(DocumentExceptions.UserNotExist);

        }

        protected override bool PreValidate(ValidationContext<CreateDocumentRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, DocumentExceptions.RequiredObject));
                return false;
            }
            return true;
        }
    }
}

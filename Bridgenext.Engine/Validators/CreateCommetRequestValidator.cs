using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using FluentValidation;
using FluentValidation.Results;

namespace Bridgenext.Engine.Validators
{
    public class CreateCommetRequestValidator : AbstractValidator<CreateCommetRequest>
    {
        public CreateCommetRequestValidator(IUserRepository userRepository, IDocumentRepositoty documentRepository)
        {

            RuleFor(x => x.IdDocument).Must(y => y != Guid.Empty)
                .WithMessage(CommentExceptions.RequiredIdDocument);

            RuleFor(x => x.IdDocument).Must(y => documentRepository.IdExistsAsync(y).Result)
                .When(z => z.IdDocument !=  Guid.Empty)
                .WithMessage(CommentExceptions.DocumentNotExist);

            RuleFor(x => x.Content).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(CommentExceptions.RequiredContent);

            RuleFor(x => x.CreateUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(CommentExceptions.CreateUserNotExist);

            RuleFor(x => x.CreateUser).Must(y => userRepository.IdExistsAsync(y).Result)
                .When(z => !string.IsNullOrEmpty(z.CreateUser))
                .WithMessage(CommentExceptions.CreateUserNotExist);


        }

        protected override bool PreValidate(ValidationContext<CreateCommetRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, CommentExceptions.RequiredObject));
                return false;
            }
            return true;
        }
    }
}

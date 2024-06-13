using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.Repositories;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;

namespace Bridgenext.Engine.Validators
{
    public class DisableDocumentRequestValidator : AbstractValidator<DisableDocumentRequest>
    {
        public DisableDocumentRequestValidator(IUserRepository userRepository, IDocumentRepositoty documentRepository)
        {
            RuleFor(x => x.Id).Must(y => y != Guid.Empty)
                .WithMessage(DocumentExceptions.RequiredId);

            RuleFor(x => x.Id).Must(y => documentRepository.IdExistsAsync(y).Result)
                .When(z => z.Id != Guid.Empty)
                .WithMessage(DocumentExceptions.DocumentNotExist);

            RuleFor(x => x.ModifyUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(DocumentExceptions.DocumentDisabled);

            RuleFor(x => x.Comment).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(DocumentExceptions.RequiredComment);

            RuleFor(x => x.ModifyUser).Must(y => userRepository.GetByCriteria(p => p.Email.ToLower().Equals(y.ToLower())).Result.FirstOrDefault()?.IdUserType == (int)UsersTypeEnum.Administrator)
                 .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(DocumentExceptions.DocumentDisabled);
        }

        protected override bool PreValidate(ValidationContext<DisableDocumentRequest> context, ValidationResult result)
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

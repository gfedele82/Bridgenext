using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace Bridgenext.Engine.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

        public CreateUserRequestValidator(IUserRepository userRepository)
        {

            RuleFor(x => x.FirstName).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredFirstName);

            RuleFor(x => x.LastName).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredLastName);

            RuleFor(x => x.Email).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredEmail);

            RuleFor(x => x.IdUserType).Must(y => Enum.GetName(typeof(UsersTypeEnum), y) != null)
                .WithMessage(UserExceptions.RequiredUserType);

            RuleFor(x => x.CreateUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.CreateUserNotExist);

            RuleFor(x => x.Email).Must(y => validateEmailRegex.IsMatch(y))
                .When(z => !string.IsNullOrEmpty(z.Email))
                .WithMessage(UserExceptions.InvalidEmail);

            RuleFor(x => x.Email).Must(y => !userRepository.IdExistsAsync(y).Result)
                .When(z => !string.IsNullOrEmpty(z.Email) && validateEmailRegex.IsMatch(z.Email))
            .WithMessage(UserExceptions.UserExist);

            RuleFor(x => x.CreateUser).Must(y => userRepository.GetByCriteria(p => p.Email.ToLower().Equals(y.ToLower())).Result.FirstOrDefault()?.IdUserType == (int)UsersTypeEnum.Administrator)
             .When(z => !string.IsNullOrEmpty(z.CreateUser))
            .WithMessage(UserExceptions.CreateUserNotExist);

        }


        protected override bool PreValidate(ValidationContext<CreateUserRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, UserExceptions.RequiredObject));
                return false;
            }
            return true;
        }
    }
}

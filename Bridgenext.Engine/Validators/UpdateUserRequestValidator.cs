using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Bridgenext.Engine.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
        private readonly IUserRepository _userRepository;

        public UpdateUserRequestValidator(IUserRepository userRepository, IConfigurationRoot _configuration)
        {
            _userRepository = userRepository;
            Guid.TryParse(_configuration["IdUserAdmin"], out Guid IdUserAdmin);

            RuleFor(x => x.Id).Must(y => y != Guid.Empty)
                .WithMessage(UserExceptions.RequiredIdUser);

            RuleFor(x => x.Id).Must(y => y != IdUserAdmin)
                .When(z => z.Id != Guid.Empty)
            .WithMessage(UserExceptions.CanNotDeleteUserAdmin);

            RuleFor(x => x.FirstName).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredFirstName);

            RuleFor(x => x.LastName).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredLastName);

            RuleFor(x => x.Email).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredEmail);

            RuleFor(x => x.IdUserType).Must(y => Enum.GetName(typeof(UsersTypeEnum), y) != null)
                .WithMessage(UserExceptions.RequiredUserType);

            RuleFor(x => x.ModifyUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.CreateUserNotExist);

            RuleFor(x => x.Email).Must(y => validateEmailRegex.IsMatch(y))
                .When(z => !string.IsNullOrEmpty(z.Email))
                .WithMessage(UserExceptions.InvalidEmail);

            RuleFor(x => x.Email).Must(y => userRepository.IdExistsAsync(y).Result)
                .When(z => !string.IsNullOrEmpty(z.Email) && validateEmailRegex.IsMatch(z.Email))
                .WithMessage(UserExceptions.UserNotExist);

            RuleFor(x => new { UserModify = x.ModifyUser, Id = x.Id}).Must(y => VerifyUser(y.UserModify, y.Id).Result)
                .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(UserExceptions.CreateUserNotExist);

        }

        private async Task<bool> VerifyUser(string userModify, Guid userIdModfy)
        {
            bool response = true;

            var user = (await _userRepository.GetByCriteria(p => p.Email.ToLower().Equals(userModify.ToLower()))).FirstOrDefault();

            if (user == null)
                return false;

            if (!(user.IdUserType == (int)UsersTypeEnum.Administrator || user.Id == userIdModfy))
                response = false;

            return true;
        }


        protected override bool PreValidate(ValidationContext<UpdateUserRequest> context, ValidationResult result)
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

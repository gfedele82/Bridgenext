using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Bridgenext.Engine.Validators
{
    public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserRequestValidator(IUserRepository userRepository, IConfigurationRoot _configuration)
        {
            Guid.TryParse(_configuration["IdUserAdmin"], out Guid IdUserAdmin);

            RuleFor(x => x.Id).Must(y => y != Guid.Empty)
                .WithMessage(UserExceptions.RequiredIdUser);

            RuleFor(x => x.Id).Must(y => y != IdUserAdmin)
                .When(z => z.Id != Guid.Empty)
            .WithMessage(UserExceptions.CanNotDeleteUserAdmin);

            RuleFor(x => x.ModifyUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.CreateUserNotExist);

            RuleFor(x => x.ModifyUser).Must(y => userRepository.GetByCriteria(p => p.Email.ToLower().Equals(y.ToLower())).Result.FirstOrDefault()?.IdUserType == (int)UsersTypeEnum.Administrator)
                 .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(UserExceptions.CreateUserNotExist);
        }
    }
}

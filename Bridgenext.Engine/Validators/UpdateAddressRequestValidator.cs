using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.Repositories;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace Bridgenext.Engine.Validators
{
    public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
    {
        public UpdateAddressRequestValidator(IAddressRepository addressRepository)
        {
            RuleFor(x => x.Id).Must(y => y != Guid.Empty)
                .WithMessage(AddressExceptions.RequiredIdAddress);

            RuleFor(x => x.Line1).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(AddressExceptions.RequiredLine1);

            RuleFor(x => x.City).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(AddressExceptions.RequiredCity);

            RuleFor(x => x.Country).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(AddressExceptions.RequiredCountry);

            RuleFor(x => x.Zip).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(AddressExceptions.RequiredZip);

            RuleFor(x => x.ModifyUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(AddressExceptions.CreateUserNotExist);

            RuleFor(x => x.Id).Must(y => addressRepository.IdExistsAsync(y).Result)
                .When(z => z.Id != Guid.Empty)
                .WithMessage(AddressExceptions.AddressNotExist);

        }

        protected override bool PreValidate(ValidationContext<UpdateAddressRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, AddressExceptions.RequiredObject));
                return false;
            }
            return true;
        }
    }
}

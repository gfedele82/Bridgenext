using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Bridgenext.Engine.Validators
{
    public class EmailRequestValidator : AbstractValidator<string>
    {
        Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

        public EmailRequestValidator()
        {

            RuleFor(x => x).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(UserExceptions.RequiredEmail);

            RuleFor(x => x).Must(y => validateEmailRegex.IsMatch(y))
                .When(z => !string.IsNullOrEmpty(z))
                .WithMessage(UserExceptions.InvalidEmail);
        }
    }
}

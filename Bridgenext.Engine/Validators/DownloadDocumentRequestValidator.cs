using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using FluentValidation;

namespace Bridgenext.Engine.Validators
{
    public class DownloadDocumentRequestValidator : AbstractValidator<Guid>
    {
        public DownloadDocumentRequestValidator(IDocumentRepositoty documentRepository)
        {
            RuleFor(x => x).Must(y => y != Guid.Empty)
               .WithMessage(DocumentExceptions.RequiredId);

            RuleFor(x => x).Must(y => ! string.IsNullOrEmpty(documentRepository.GetAsync(y).Result.TargetFile))
                .When(z => z != Guid.Empty)
                .WithMessage(DocumentExceptions.FileDoesNotHaveFile);
        }
    }
}

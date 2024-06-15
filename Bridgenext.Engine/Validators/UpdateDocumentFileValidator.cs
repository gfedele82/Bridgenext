using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Bridgenext.Models.Enums;

namespace Bridgenext.Engine.Validators
{
    public class UpdateDocumentFileValidator : AbstractValidator<UpdateDocumentFileRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepositoty _documentRepository;
        private readonly IConfiguration _configuration;

        public UpdateDocumentFileValidator(IUserRepository userRepository, IDocumentRepositoty documentRepository, IConfigurationRoot configuration)
        {
            _userRepository = userRepository;
            _documentRepository = documentRepository;
            _configuration = configuration;

            RuleFor(x => x.Id).Must(y => y != Guid.Empty)
                .WithMessage(DocumentExceptions.RequiredId);

            RuleFor(x => x.Id).Must(y => documentRepository.IdExistsAsync(y).Result)
                .When(z => z.Id != Guid.Empty)
                .WithMessage(DocumentExceptions.DocumentNotExist);

            RuleFor(x => x.Id).Must(y => documentRepository.GetAsync(y).Result.DocumentType.Id != (int)FileTypes.Text)
                .When(z => z.Id != Guid.Empty && documentRepository.IdExistsAsync(z.Id).Result && !string.IsNullOrEmpty(z.File))
                .WithMessage(DocumentExceptions.DocumentNotMatch);


            RuleFor(x => x.ModifyUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(DocumentExceptions.CreateUserNotExist);

            RuleFor(x => x.ModifyUser).Must(y => userRepository.IdExistsAsync(y).Result)
                .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(DocumentExceptions.UserNotExist);

            RuleFor(x => new { UserModify = x.ModifyUser, Id = x.Id }).Must(y => VerifyUser(y.UserModify, y.Id).Result)
                .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(DocumentExceptions.CreateUserNotExist);

            RuleFor(x => x.File).Must(y => File.Exists(y))
                .When(z => !string.IsNullOrEmpty(z.File))
                .WithMessage(DocumentExceptions.FileNotExist);

        }

        private async Task<bool> VerifyUser(string userModify, Guid documentId)
        {
            bool response = true;

            Guid.TryParse(_configuration["IdUserAdmin"], out Guid IdUserAdmin);

            var user = (await _userRepository.GetByCriteria(p => p.Email.ToLower().Equals(userModify.ToLower()))).FirstOrDefault();

            var document = (await _documentRepository.GetAsync(documentId));

            if (user == null)
                return false;

            if (!(document.Users.Id == IdUserAdmin || document.Users.Id == user.Id))
                response = false;

            return response;
        }

        protected override bool PreValidate(ValidationContext<UpdateDocumentFileRequest> context, ValidationResult result)
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

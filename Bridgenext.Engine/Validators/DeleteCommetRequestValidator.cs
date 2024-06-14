using Bridgenext.DataAccess.Interfaces;
using Bridgenext.Models.Constant.Exceptions;
using Bridgenext.Models.DTO.Request;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Bridgenext.Engine.Validators
{
    public class DeleteCommetRequestValidator : AbstractValidator<DeleteCommetRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IConfiguration _configuration;

        public DeleteCommetRequestValidator(IConfigurationRoot configuration, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
           _commentRepository = commentRepository;

            RuleFor(x => x.Id).Must(y => y != Guid.Empty)
                .WithMessage(CommentExceptions.RequiredId);

            RuleFor(x => x.Id).Must(y => commentRepository.IdExistsAsync(y).Result)
                .When(z => z.Id != Guid.Empty)
                .WithMessage(CommentExceptions.CommentNotExist);


            RuleFor(x => x.ModifyUser).Must(y => !string.IsNullOrEmpty(y))
                .WithMessage(CommentExceptions.CreateUserNotExist);

            RuleFor(x => x.ModifyUser).Must(y => userRepository.IdExistsAsync(y).Result)
                .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(CommentExceptions.CreateUserNotExist);

            RuleFor(x => new { UserModify = x.ModifyUser, Id = x.Id }).Must(y => VerifyUser(y.UserModify, y.Id).Result)
                .When(z => !string.IsNullOrEmpty(z.ModifyUser))
                .WithMessage(DocumentExceptions.CreateUserNotExist);


        }

        private async Task<bool> VerifyUser(string userModify, Guid commentId)
        {
            bool response = true;

            Guid.TryParse(_configuration["IdUserAdmin"], out Guid IdUserAdmin);

            var user = (await _userRepository.GetByCriteria(p => p.Email.ToLower().Equals(userModify.ToLower()))).FirstOrDefault();

            var document = (await _commentRepository.GetAsync(commentId));

            if (user == null)
                return false;

            if (!(document.Users.Id == IdUserAdmin || document.Users.Id == user.Id))
                response = false;

            return true;
        }

        protected override bool PreValidate(ValidationContext<DeleteCommetRequest> context, ValidationResult result)
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

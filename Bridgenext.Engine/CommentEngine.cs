using Bridgenext.DataAccess.Interfaces;
using Bridgenext.DataAccess.DTOAdapter;
using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Bridgenext.Engine.Validators;

namespace Bridgenext.Engine
{
    public class CommentEngine (ILogger<CommentEngine> _logger,
        IValidator<CreateCommetRequest> _createCommentRequestValidator,
        IValidator<DeleteCommetRequest> _deleteCommentRequestValidator,
        ICommentRepository _commentRepository,
        IDocumentRepositoty _documentRepository,
        IUserRepository _userRepository) : ICommentEngine
    {
        public async Task<CommentDto> CreateComment(CreateCommetRequest addCommentRequest)
        {
            _logger.LogInformation($"CreateComment: Payload = {JsonConvert.SerializeObject(addCommentRequest)}");

            await _createCommentRequestValidator.ValidateAndThrowAsync(addCommentRequest);

            var user = (await _userRepository.GetAllByEmail(addCommentRequest.CreateUser)).FirstOrDefault();

            var document = await _documentRepository.GetAsync(addCommentRequest.IdDocument);

            var dbComment = addCommentRequest.ToDatabaseModel(document, user);

            dbComment = await _commentRepository.InsertAsync(dbComment);

            return dbComment.ToDomainModel();
        }

        public async Task<CommentDto> GetCommentById(Guid id)
        {
            _logger.LogInformation($"GetCommentById: Id = {id}");

            var dbComment = await _commentRepository.GetAsync(id);

            return dbComment.ToDomainModel();

        }

        public async Task<List<CommentDto>> GetAllComments()
        {
            _logger.LogInformation($"GetAllComments");

            var dbComment = await _commentRepository.GetAll();

            return dbComment.ToDomainModel().ToList();

        }

        public async Task<CommentDto> DeleteComment(DeleteCommetRequest deleteComment)
        {
            await _deleteCommentRequestValidator.ValidateAndThrowAsync(deleteComment);

            _logger.LogInformation($"DeleteUser: Payload = {JsonConvert.SerializeObject(deleteComment)}");

            var existingComment = await _commentRepository.GetAsync(deleteComment.Id);

            if (existingComment == null)
                return null;

            await _commentRepository.DeleteAsync(existingComment);

            return existingComment.ToDomainModel();
        }
    }
}

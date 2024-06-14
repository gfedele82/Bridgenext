using Bridgenext.Models.DTO.Request;
using Bridgenext.Models.DTO.Response;

namespace Bridgenext.Engine.Interfaces
{
    public interface ICommentEngine
    {
        Task<CommentDto> CreateComment(CreateCommetRequest addCommentRequest);

        Task<CommentDto> GetCommentById(Guid id);

        Task<List<CommentDto>> GetAllComments();

        Task<CommentDto> DeleteComment(DeleteCommetRequest deleteComment);
    }
}

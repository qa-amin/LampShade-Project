using _0_Framework.Domain;
using CommentManagement.Application.Contracts.Comment;

namespace CommentManagement.Domain.CommentAgg
{
    public interface ICommentRepository : IRepository<long, Comment>
    {
        Task<List<CommentViewModel>> Search(CommentSearchModel searchModel);
    }
}

using _0_Framework.Application;

namespace CommentManagement.Application.Contracts.Comment
{
    public interface ICommentApplication
    {
        Task<OperationResult> Add(AddComment command);
        Task<OperationResult> Confirm(long id);
        Task<OperationResult> Cancel(long id);
        Task<List<CommentViewModel>> Search(CommentSearchModel searchModel);
    }
}

using _0_Framework.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;

namespace CommentManagement.Application
{
    public class CommentApplication : ICommentApplication
    {
        private readonly ICommentRepository _commentRepository;

        public CommentApplication(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<OperationResult> Add(AddComment command)
        {
            var operation = new OperationResult();
            var comment = new Comment(command.Name, command.Email, command.Website, command.Message, 
                command.OwnerRecordId, command.Type, command.ParentId);

            await _commentRepository.Create(comment);
            await _commentRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<OperationResult> Cancel(long id)
        {
            var operation = new OperationResult();
            var comment = await _commentRepository.Get(id);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            comment.Cancel();
            await _commentRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<OperationResult> Confirm(long id)
        {
            var operation = new OperationResult();
            var comment = await _commentRepository.Get(id);
            if (comment == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            comment.Confirm();
            await _commentRepository.SaveChanges();
            return operation.Succeeded();
        }

        public async Task<List<CommentViewModel>> Search(CommentSearchModel searchModel)
        {
            return await _commentRepository.Search(searchModel);
        }
    }
}

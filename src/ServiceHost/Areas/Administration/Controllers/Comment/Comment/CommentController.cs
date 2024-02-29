using CommentManagement.Application.Contracts.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Controllers.Comment.Comment
{
    [Authorize(Policy = "AdminArea")]
    public class CommentController : Controller
    {
        private readonly ICommentApplication _commentApplication;

        [TempData]
        public string Message { get; set; }

        public CommentController(ICommentApplication commentApplication)
        {
            _commentApplication = commentApplication;
        }

        [Area("Administration")]
        [Route("admin/comments")]
        [HttpGet]
        public async Task<IActionResult> Index(string? Name, string? Email)
        {
            var searchComment = new CommentSearchModel
            {
                Name = Name,
                Email = Email
            };
           var Comments = await _commentApplication.Search(searchComment);

            return View(Comments);
        }

        [Area("Administration")]
        [Route("admin/comment/Confirm")]
        [HttpGet]
        public async Task<IActionResult> Confirm(long id)
        {
            var result = await _commentApplication.Confirm(id);
            if (result.IsSucceeded)
                return new RedirectResult("./index");

            Message = result.Message;
            return new RedirectResult("./index");

            
        }
        [Area("Administration")]
        [Route("admin/comment/Cancel")]
        [HttpGet]
        public async Task<IActionResult> Cancel(long id)
        {
            var result = await _commentApplication.Cancel(id);
            if (result.IsSucceeded)
                return new RedirectResult("./index");

            Message = result.Message;
            return new RedirectResult("./index");


        }

    }
}

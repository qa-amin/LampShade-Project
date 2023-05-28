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
        [Route("admin/comment/comment/index")]
        [HttpGet]
        public IActionResult Index(string? Name, string? Email)
        {
            var searchComment = new CommentSearchModel
            {
                Name = Name,
                Email = Email
            };
           var Comments = _commentApplication.Search(searchComment);

            return View(Comments);
        }

        [Area("Administration")]
        [Route("admin/comment/comment/Confirm")]
        [HttpGet]
        public IActionResult Confirm(long id)
        {
            var result = _commentApplication.Confirm(id);
            if (result.IsSucceeded)
                return new RedirectResult("./index");

            Message = result.Message;
            return new RedirectResult("./index");

            
        }
        [Area("Administration")]
        [Route("admin/comment/comment/Cancel")]
        [HttpGet]
        public IActionResult Cancel(long id)
        {
            var result = _commentApplication.Cancel(id);
            if (result.IsSucceeded)
                return new RedirectResult("./index");

            Message = result.Message;
            return new RedirectResult("./index");


        }

    }
}

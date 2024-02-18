using _1_LampShadeQuery.Contracts.Product;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductQuery _productQuery;
        private readonly ICommentApplication _commentApplication;

        public ProductController(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
        }
        [HttpGet]
        [Route("product/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var product = await _productQuery.GetProductDetails(id);
            ViewBag.Product = product;
            return View();
        }
        [HttpPost]
        [Route("product/add-comment")]
        public async Task<IActionResult> Create(AddComment command, string productSlug)
        {
            command.Type = CommentType.Product;
            var result = await _commentApplication.Add(command);
            return RedirectToAction("Index", new { id = productSlug });
        }
    }
}

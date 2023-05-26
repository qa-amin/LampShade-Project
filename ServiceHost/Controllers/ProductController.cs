using _1_LampShadeQuery.Contracts.Product;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ProductController : Controller
    {
        public ProductQueryModel Product;
        private readonly IProductQuery _productQuery;
        private readonly ICommentApplication _commentApplication;

        public ProductController(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
        }
        [HttpGet]
        [Route("product/index/{id}")]
        public IActionResult Index(string id)
        {
            Product = _productQuery.GetProductDetails(id);
            ViewBag.Product = Product;
            return View();
        }
        [HttpPost]
        [Route("product/Create")]
        public IActionResult Create(AddComment command, string productSlug)
        {
            command.Type = CommentType.Product;
            var result = _commentApplication.Add(command);
            return RedirectToAction("Index", new { id = productSlug });
        }
    }
}

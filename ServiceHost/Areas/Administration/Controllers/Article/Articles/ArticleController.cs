using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.CompilerServices;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Controllers.Article.Articles
{
    public class ArticleController : Controller
    {
        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        private  List<ArticleViewModel> _articleViewModels = new List<ArticleViewModel> ();

        public ArticleSearchModel searchModel;

        public ArticleController(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
        }


        [Area("Administration")]
        [Route("admin/article/article/index")]
        [HttpGet]
        public IActionResult Index(string? title, long categoryId)
        {
            var SearchModel = new ArticleSearchModel()
            {
                Title = title,
                CategoryId = categoryId
            };
            _articleViewModels = _articleApplication.Search(SearchModel);


            var articleCategories = new SelectList(_articleCategoryApplication.GetArticleCategories(), "Id", "Name");
            ViewBag.ArticleCategories = articleCategories;
            return View(_articleViewModels);
        }


        [Area("Administration")]
        [Route("admin/article/article/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var command = new CreateArticle();
            command.ArticleCategories = _articleCategoryApplication.GetArticleCategories();
            return View("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/article/article/Create")]
        [HttpPost]
        public IActionResult Create(CreateArticle commend)
        {
            var result = _articleApplication.Create(commend);

            return new RedirectResult("./index");
        }


        [Area("Administration")]
        [Route("admin/article/article/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editArticle= _articleApplication.GetDetails(Id);
            editArticle.ArticleCategories = _articleCategoryApplication.GetArticleCategories();
            return View("_Edit", editArticle);
        }

        [Area("Administration")]
        [Route("admin/article/article/Edit")]
        
        public IActionResult Edit(EditArticle commend)
        {
            var result = _articleApplication.Edit(commend);
            return new RedirectResult("./index");
        }
    }
}

using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Controllers.Article.ArticleCategory
{
    [Authorize(Policy = "Administration")]
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryApplication _articleCategoryApplication;

        private  List<ArticleCategoryViewModel> _articleCategoryViewModels = new List<ArticleCategoryViewModel> ();

        public ArticleCategorySearchModel searchModel;

        public ArticleCategoryController(IArticleCategoryApplication articleCategoryApplication)
        {
            _articleCategoryApplication = articleCategoryApplication;
        }


        [Area("Administration")]
        [Route("admin/article/articlecategory/index")]
        [HttpGet]
        public IActionResult Index(string? searchModel)
        {
            var SearchModel = new ArticleCategorySearchModel()
            {
                Name = searchModel
            };
            _articleCategoryViewModels = _articleCategoryApplication.Search(SearchModel);
            return View(_articleCategoryViewModels);
        }


        [Area("Administration")]
        [Route("admin/article/articlecategory/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new CreateArticleCategory());
        }

        [Area("Administration")]
        [Route("admin/article/articlecategory/Create")]
        [HttpPost]
        public JsonResult Create(CreateArticleCategory commend)
        {
            var result = _articleCategoryApplication.Create(commend);

            return new JsonResult(result);
        }


        [Area("Administration")]
        [Route("admin/article/articlecategory/Edit")]
        [HttpGet]
        public IActionResult Edit(long Id)
        {
            var editArticleCategory = _articleCategoryApplication.GetDetails(Id);
            return PartialView("_Edit", editArticleCategory);
        }

        [Area("Administration")]
        [Route("admin/article/articlecategory/Edit")]
        
        public JsonResult Edit(EditArticleCategory commend)
        {
            var result = _articleCategoryApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}

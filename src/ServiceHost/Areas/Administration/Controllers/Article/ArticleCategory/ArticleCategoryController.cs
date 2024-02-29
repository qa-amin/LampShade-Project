using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Route("admin/articlecategories")]
        [HttpGet]
        public async Task<IActionResult> Index(string? searchModel)
        {
            var SearchModel = new ArticleCategorySearchModel()
            {
                Name = searchModel
            };
            _articleCategoryViewModels = await _articleCategoryApplication.Search(SearchModel);
            return View(_articleCategoryViewModels);
        }


        [Area("Administration")]
        [Route("admin/articlecategory/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return PartialView("_Create", new CreateArticleCategory());
        }

        [Area("Administration")]
        [Route("admin/articlecategory/Create")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateArticleCategory commend)
        {
            var result = await _articleCategoryApplication.Create(commend);

            return new JsonResult(result);
        }


        [Area("Administration")]
        [Route("admin/articlecategory/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var editArticleCategory = await _articleCategoryApplication.GetDetails(Id);
            return PartialView("_Edit", editArticleCategory);
        }

        [Area("Administration")]
        [Route("admin/articlecategory/Edit")]
        
        public async Task<JsonResult> Edit(EditArticleCategory commend)
        {
            var result = await _articleCategoryApplication.Edit(commend);
            return new JsonResult(result);
        }
    }
}

using BlogManagement.Application.Contracts.Article;
using BlogManagement.Application.Contracts.ArticleCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ServiceHost.Areas.Administration.Controllers.Article.Articles
{
    [Authorize(Policy = "AdminArea")]
    public class ArticleController : Controller
    {
        private readonly IArticleApplication _articleApplication;
        private readonly IArticleCategoryApplication _articleCategoryApplication;


        public ArticleController(IArticleApplication articleApplication, IArticleCategoryApplication articleCategoryApplication)
        {
            _articleApplication = articleApplication;
            _articleCategoryApplication = articleCategoryApplication;
        }


        [Area("Administration")]
        [Route("admin/articles")]
        [HttpGet]
        public async Task<IActionResult> Index(string? title, long categoryId)
        {
            var SearchModel = new ArticleSearchModel()
            {
                Title = title,
                CategoryId = categoryId
            };
            var articleViewModels = await _articleApplication.Search(SearchModel);


            var articleCategories = new SelectList(await _articleCategoryApplication.GetArticleCategories(), "Id", "Name");
            ViewBag.ArticleCategories = articleCategories;
            return View(articleViewModels);
        }


        [Area("Administration")]
        [Route("admin/article/Create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var command = new CreateArticle();
            command.ArticleCategories = await _articleCategoryApplication.GetArticleCategories();
            return View("_Create", command);
        }

        [Area("Administration")]
        [Route("admin/article/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateArticle commend)
        {
            var result = await _articleApplication.Create(commend);

            return new RedirectResult("./index");
        }


        [Area("Administration")]
        [Route("admin/article/Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var editArticle=  await _articleApplication.GetDetails(Id);
            editArticle.ArticleCategories = await _articleCategoryApplication.GetArticleCategories();
            return View("_Edit", editArticle);
        }

        [Area("Administration")]
        [Route("admin/article/Edit")]
        
        public async Task<IActionResult> Edit(EditArticle commend)
        {
            var result =  await _articleApplication.Edit(commend);
            return new RedirectResult("./index");
        }
    }
}

using _1_LampShadeQuery.Contracts.Slide;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace ServiceHost.ViewComponents
{
    public class SlideViewComponents : ViewComponent
    {
        private readonly ISlideQuery _slideQuery;

        public SlideViewComponents(ISlideQuery slideQuery)
        {
            _slideQuery = slideQuery;
        }

        public async Task<IViewComponentResult> InvokeTask()
        {
            var slides = await _slideQuery.GetSlides();

            return View(slides);
        }
    }
}

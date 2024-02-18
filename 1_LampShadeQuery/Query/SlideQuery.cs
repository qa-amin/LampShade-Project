using _1_LampShadeQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
namespace _1_LampShadeQuery.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopManagementDbContext _shopManagementDbContext;

        public SlideQuery(ShopManagementDbContext shopManagementDbContext)
        {
            _shopManagementDbContext = shopManagementDbContext;
        }

        public async Task<List<SlideQueryModel>> GetSlides()
        {
            var slides = await _shopManagementDbContext.Slides.Where(x => x.IsRemoved == false).ToListAsync();

            return slides.Select(x => new SlideQueryModel
            {
                BtnText = x.BtnText,
                Heading = x.Heading,
                IsRemoved = x.IsRemoved,
                Link = x.Link,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Text = x.Text,
                Title = x.Title,

            }).ToList();

            


        }
    }
}

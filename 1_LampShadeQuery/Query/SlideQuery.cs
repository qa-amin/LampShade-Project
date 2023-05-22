using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _1_LampShadeQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EFCore;

namespace _1_LampShadeQuery.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopManagementDbContext _shopManagementDbContext;

        public SlideQuery(ShopManagementDbContext shopManagementDbContext)
        {
            _shopManagementDbContext = shopManagementDbContext;
        }

        public List<SlideQueryModel> GetSlides()
        {
            return _shopManagementDbContext.Slides.Where(x => x.IsRemoved == false).Select(x => new SlideQueryModel
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

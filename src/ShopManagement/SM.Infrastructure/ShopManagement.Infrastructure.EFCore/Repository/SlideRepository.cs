using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class SlideRepository : RepositoryBase<long , Slide>, ISlideRepository
    {
        private readonly ShopManagementDbContext _context;
        public SlideRepository(ShopManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EditSlide> GetDetails(long id)
        {
            var slide = await _context.Slides.FindAsync(id);

            return new EditSlide()
            {
                BtnText = slide.BtnText,
                Heading = slide.Heading,
                Title = slide.Title,
                Text = slide.Text,
                Id = slide.Id,
                IsRemoved = slide.IsRemoved,
                PictureTitle = slide.PictureTitle,
                PictureAlt = slide.PictureAlt,
                Link = slide.Link,
            };
        }

        public async Task<List<SlideViewModel>> GetList()
        {
             return await _context.Slides.Select(x => new SlideViewModel
            {
                Id = x.Id,
                Heading = x.Heading,
                Picture = x.Picture,
                Title = x.Title,
                IsRemoved = x.IsRemoved,
                CreationDate = x.CreationDate.ToFarsi()
            }).OrderByDescending(x => x.Id).ToListAsync();
        }
    }
}

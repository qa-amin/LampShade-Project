using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Slide;

namespace ShopManagement.Domain.SlideAgg
{
    public interface ISlideRepository : IRepository<long, Slide>
    {
        Task<EditSlide> GetDetails(long id);
        Task<List<SlideViewModel>> GetList();
    }
}

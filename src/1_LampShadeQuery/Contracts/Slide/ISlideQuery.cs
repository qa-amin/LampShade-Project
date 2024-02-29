namespace _1_LampShadeQuery.Contracts.Slide
{
    public interface ISlideQuery
    {
        Task<List<SlideQueryModel>> GetSlides();
    }
}

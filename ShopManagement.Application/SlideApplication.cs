using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application
{
    public class SlideApplication : ISlideApplication
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IFileUploader _fileUploader;

        public SlideApplication(ISlideRepository slideRepository, IFileUploader fileUploader)
        {
            _slideRepository = slideRepository;
            _fileUploader = fileUploader;
        }

        public async Task<OperationResult> Create(CreateSlide command)
        {
            var operationResult = new OperationResult();

            var path = "slides";
            var picturePath = _fileUploader.Upload(command.Picture, path);

            var slide = new Slide(picturePath, command.PictureAlt, command.PictureTitle, command.Heading,
                command.Title, command.Text, command.BtnText, command.Link);
            await _slideRepository.Create(slide);
            await _slideRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Edit(EditSlide command)
        {
            var operationResult = new OperationResult();
            var getDetailSlide = await _slideRepository.Get(command.Id);
            if (getDetailSlide == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }
            var path = "slides";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            getDetailSlide.Edit(picturePath, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.BtnText, command.Link); 
            await _slideRepository.SaveChanges();

            return operationResult.Succeeded();

        }

        public async Task<OperationResult> Remove(long id)
        {
            var operationResult = new OperationResult();
            var slide = await _slideRepository.Get(id);
            if (slide == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            slide.Remove();
            await _slideRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Restore(long id)
        {
            var operationResult = new OperationResult();
            var slide = await _slideRepository.Get(id);
            if (slide == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            slide.Restore();
            await _slideRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<EditSlide> GetDetails(long id)
        {
            return await _slideRepository.GetDetails(id);
        }

        public async Task<List<SlideViewModel>> GetList()
        {
             return await _slideRepository.GetList();
        }
    }
}

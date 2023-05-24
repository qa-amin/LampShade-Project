using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public OperationResult Create(CreateSlide command)
        {
            var operationResult = new OperationResult();

            var path = "slides";
            var picturePath = _fileUploader.Upload(command.Picture, path);

            var slide = new Slide(picturePath, command.PictureAlt, command.PictureTitle, command.Heading,
                command.Title, command.Text, command.BtnText, command.Link);
            _slideRepository.Create(slide);
            _slideRepository.SaveChanges();

            return operationResult.Succeeded();
        }

        public OperationResult Edit(EditSlide command)
        {
            var operationResult = new OperationResult();
            var getDetailSlide = _slideRepository.GetDetails(command.Id);
            if (getDetailSlide == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }
            var path = "slides";
            var picturePath = _fileUploader.Upload(command.Picture, path);
            getDetailSlide.Edit(picturePath, command.PictureAlt, command.PictureTitle, command.Heading, command.Title, command.Text, command.BtnText, command.Link); 
            _slideRepository.SaveChanges();

            return operationResult.Succeeded();

        }

        public OperationResult Remove(long id)
        {
            var operationResult = new OperationResult();
            var slide = _slideRepository.Get(id);
            if (slide == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            slide.Remove();
            _slideRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Restore(long id)
        {
            var operationResult = new OperationResult();
            var slide = _slideRepository.Get(id);
            if (slide == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            slide.Restore();
            _slideRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public EditSlide GetDetails(long id)
        {
            var slide = _slideRepository.GetDetails(id);
            var editSlide = new EditSlide
            {
                BtnText = slide.BtnText,
                Heading = slide.Heading,
                Title = slide.Title,
                Text = slide.Text,
                Id = slide.Id,
                IsRemoved = slide.IsRemoved,
               // Picture = slide.Picture,
                PictureTitle = slide.PictureTitle,
                PictureAlt = slide.PictureAlt,
                Link = slide.Link,
                
            };
            return editSlide;
        }

        public List<SlideViewModel> GetList()
        {
           var listSlide = _slideRepository.Get().ToList();
           var listSlideViewModel = listSlide.Select(x => new SlideViewModel
           {
               Heading = x.Heading,
               Title = x.Title,
               Id = x.Id,
               Picture = x.Picture,
               IsRemoved = x.IsRemoved,
               CreationDate = x.CreationDate.ToFarsi(),
               
           }).ToList();

           return listSlideViewModel;
        }
    }
}

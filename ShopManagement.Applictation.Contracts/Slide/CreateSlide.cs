using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Application.Contracts.Slide
{
    public class CreateSlide
    {
        public IFormFile Picture { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PictureAlt { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PictureTitle { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Heading { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Title { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Text { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string BtnText { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Link { get;  set; }
        public bool IsRemoved { get;  set; }
    }
}

using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Application.Contracts.ArticleCategory
{
    public class CreateArticleCategory
    {
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Name { get; set; }
        
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PictureAlt { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Description { get; set; }
        
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public int ShowOrder { get; set; }
        
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Keywords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string MetaDescription { get; set; }

        public string? CanonicalAddress { get; set; }
    }
}

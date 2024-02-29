using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.ProductCategory;


namespace ShopManagement.Application.Contracts.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Name { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Code { get;  set; }
        public string? ShortDescription { get;  set; }
        public string? Description { get;  set; }
        [FileExtentionLimitationAttribute(new string[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = ValidationMessages.InValidFileFormat)]
        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile? Picture { get;  set; }
        public string? PictureAlt { get;  set; }
        public string? PictureTitle { get;  set; }
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Slug { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string KeyWords { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string MetaDescription { get;  set; }

        [Range(1,100000, ErrorMessage =ValidationMessages.IsRequierd )]
        public long CategoryId { get; set; }
        public List<ProductCategoryViewModel>? Categoreis { get; set; }
    }

   
}

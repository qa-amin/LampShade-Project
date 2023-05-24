using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.ProductCategory;


namespace ShopManagement.Application.Contracts.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Name { get;  set; }

        //[Required(ErrorMessage = ValidationMessages.IsRequierd)]
        //public double UnitPrice { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Code { get;  set; }
        public string? ShortDescription { get;  set; }
        public string? Description { get;  set; }
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

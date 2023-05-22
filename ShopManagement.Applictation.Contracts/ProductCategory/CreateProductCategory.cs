using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.ProductCategory
{
	public class CreateProductCategory
	{
		[Required(ErrorMessage = ValidationMessages.IsRequierd)]
		public string Name { get;  set; }
		public string? Description { get;  set; }
		public string? Picture { get;  set; }
		public string? PictureAlt { get;  set; }
		public string? PictureTitle { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string KeyWords { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string MetaDescription { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Slug { get;  set; }
	}
}

using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using BlogManagement.Application.Contracts.ArticleCategory;

namespace BlogManagement.Application.Contracts.Article
{
    public class CreateArticle
    {
        [MaxLength(500, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public IFormFile Picture { get; set; }

        [MaxLength(500, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PictureAlt { get; set; }

        [MaxLength(500, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string PublishDate { get; set; }

        [MaxLength(500, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Slug { get; set; }

        [MaxLength(100, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string Keywords { get; set; }

        [MaxLength(150, ErrorMessage = ValidationMessages.MaxLenght)]
        [Required(ErrorMessage = ValidationMessages.IsRequierd)]
        public string MetaDescription { get; set; }

        [MaxLength(1000, ErrorMessage = ValidationMessages.MaxLenght)]
        public string CanonicalAddress { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationMessages.IsRequierd)]
        public long CategoryId { get; set; }

        public List<ArticleCategoryViewModel> ArticleCategories { get; set; }
    }
}

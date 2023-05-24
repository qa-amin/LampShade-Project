using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore
{
    public class BlogManagementDbContext : DbContext
    {
        public DbSet<ArticleCategory> ArticleCategories { set; get; } 
        public BlogManagementDbContext(DbContextOptions<BlogManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ArticleCategoryMapping());
        }
    }
}
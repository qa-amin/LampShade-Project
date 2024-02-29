using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_LampShadeQuery.Contracts.ProductCategory
{
    public interface IProductCategoryQuery
    {
        Task<List<ProductCategoryQueryModel>> GetProductCategories();
        Task<List<ProductCategoryQueryModel>> GetProductCategoriesWithProducts();

        Task<ProductCategoryQueryModel> GetProductCategoryWithProductsBy(string slug);
    }
}

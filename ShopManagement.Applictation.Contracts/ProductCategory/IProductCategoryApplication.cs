using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;


namespace ShopManagement.Application.Contracts.ProductCategory
{
	public interface IProductCategoryApplication
	{
		OperationResult Create(CreateProductCategory command);
		OperationResult Edit(EditProductCategory command);
		ProductCategoryViewModel Get(long Id);
        List<ProductCategoryViewModel> Get();
		List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
        EditProductCategory GetDetails(long Id);
        List<ProductCategoryViewModel> GetProductCategories();


    }
}

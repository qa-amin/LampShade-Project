using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductPictureRepository _productPictureRepository;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository)
        {
            _productPictureRepository = productPictureRepository;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            var opreationResult = new OperationResult();
            if (_productPictureRepository.Exists(x => x.Picture == command.Picture && x.ProductId == command.ProductId))
            {
                return opreationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }
            var productPicture = new ProductPicture(command.ProductId,command.Picture, command.PictureTitle, command.PictureAlt);

            _productPictureRepository.Create(productPicture);

            _productPictureRepository.SaveChanges();

            return opreationResult.Succeeded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            var opreationResult = new OperationResult();

            if (_productPictureRepository.Exists(x => x.Picture == command.Picture && x.ProductId == command.ProductId && x.Id != command.Id))
            {
                return opreationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var editProductPicture = _productPictureRepository.GetDetails(command.Id);
            if (editProductPicture == null)
            {
                return opreationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            editProductPicture.Edit(command.Id, command.Picture, command.PictureAlt, command.PictureTitle);
            _productPictureRepository.SaveChanges();

            return opreationResult.Succeeded();
        }

        public OperationResult Remove(long id)
        {
            var operationResult = new OperationResult();

            var productPicture = _productPictureRepository.Get(id);

            if(productPicture == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);


            productPicture.Remove();
            _productPictureRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public OperationResult Restore(long id)
        {
            var operationResult = new OperationResult();

            var productPicture = _productPictureRepository.Get(id);

            if (productPicture == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);


            productPicture.Restore();
            _productPictureRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public EditProductPicture GetDetails(long id)
        {
            var productPicture = _productPictureRepository.GetDetails(id);

            var editProductPicture = new EditProductPicture
            {
                Id = productPicture.Id,
                Picture = productPicture.Picture,
                PictureAlt = productPicture.PictureAlt,
                PictureTitle = productPicture.PictureTitle,
                ProductId = productPicture.ProductId,

            };

            return editProductPicture;
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            var listProductPicture = _productPictureRepository.search(searchModel.ProductId);
            var listProdutPictureViewModel = listProductPicture.Select(x => new ProductPictureViewModel
            {
                Id = x.Id,
                Picture = x.Picture,
                CreationDate = x.CreationDate.ToFarsi(),
                Product = x.Product.Name,
                ProductId = x.ProductId,
                IsRemoved = x.IsRemove
                
            }).ToList();

            return listProdutPictureViewModel;
        }
    }

 
}

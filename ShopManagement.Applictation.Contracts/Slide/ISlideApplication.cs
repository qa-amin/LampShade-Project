using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application.Contracts.Slide
{
    public interface ISlideApplication
    {

        Task<OperationResult> Create(CreateSlide command);
        Task<OperationResult> Edit(EditSlide command);
        Task<OperationResult> Remove(long id);
        Task<OperationResult> Restore(long id);
        Task<EditSlide> GetDetails(long id);
        Task<List<SlideViewModel>> GetList();
    }
}

using _0_Framework.Application;
using DiscountManagement.Application.Contracts.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;

namespace DiscountManagement.Application
{
    public class ColleagueDiscountApplication : IColleagueDiscountApplication
    {
        private readonly IColleagueDiscountRepository _colleagueDiscountRepository;
        

        public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository)
        {
            _colleagueDiscountRepository = colleagueDiscountRepository;
            
        }

        public async Task<OperationResult> Define(DefineColleagueDiscount command)
        {
            var operationResult = new OperationResult();
            if (await _colleagueDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.ProductId))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

           
            var colleagueDiscount = new ColleagueDiscount(command.ProductId, command.DiscountRate);

            await _colleagueDiscountRepository.Create(colleagueDiscount);
            await _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Edit(EditColleagueDiscount command)
        {

            var operationResult = new OperationResult();

            var colleagueDiscount = await _colleagueDiscountRepository.Get(command.Id);
            if (colleagueDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }

            if (await _colleagueDiscountRepository.Exists(x =>
                    x.ProductId == command.ProductId && x.DiscountRate == command.ProductId && x.Id != command.Id))
            {
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }



            colleagueDiscount.Edit(command.Id, command.DiscountRate);
            _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Remove(long id)
        {
            var operationResult = new OperationResult();

            var colleagueDiscount = await _colleagueDiscountRepository.Get(id);
            if (colleagueDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }
           



            colleagueDiscount.Remove();
            await _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<OperationResult> Restore(long id)
        {
            var operationResult = new OperationResult();

            var colleagueDiscount = await _colleagueDiscountRepository.Get(id);
            if (colleagueDiscount == null)
            {
                return operationResult.Failed(ApplicationMessages.RecordNotFound);
            }




            colleagueDiscount.Restore();
            await _colleagueDiscountRepository.SaveChanges();
            return operationResult.Succeeded();
        }

        public async Task<List<ColleagueDiscountViewModel>> search(ColleagueDiscountSearchModel searchModel)
        {
            return await _colleagueDiscountRepository.Search(searchModel.ProductId);
        }

        public async Task<EditColleagueDiscount> GetDetails(long id)
        {
            return await _colleagueDiscountRepository.GetDetails(id);
        }
    }
}

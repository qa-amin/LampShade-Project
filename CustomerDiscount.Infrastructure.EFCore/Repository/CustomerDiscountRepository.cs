using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;


namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class CustomerDiscountRepository : RepositoryBase<long , DiscountManagement.Domain.CustomerDiscountAgg.CustomerDiscount> , ICustomerDiscountRepository
    {
        private readonly DiscountManagementDbContext _context;
        
        public CustomerDiscountRepository(DiscountManagementDbContext context) : base(context)
        {
            _context = context;
            
        }


        public List<CustomerDiscount> search(long? productId, string? startDate, string? endDate)
        {
            
            return _context.CustomerDiscounts.ToList();
        }

        public CustomerDiscount GetDetails(long id)
        {
            return _context.CustomerDiscounts.Find(id);
        }
    }
}

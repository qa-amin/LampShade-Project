using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using Microsoft.EntityFrameworkCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class ColleagueDiscountRepository : RepositoryBase<long, ColleagueDiscount>, IColleagueDiscountRepository
    {
        private readonly DiscountManagementDbContext _context;

        public ColleagueDiscountRepository(DiscountManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public List<ColleagueDiscount> Search(long id)
        {
            return _context.ColleagueDiscounts.ToList();
        }

        public ColleagueDiscount GetDetails(long id)
        {
            return _context.ColleagueDiscounts.Find(id);
        }
    }
}

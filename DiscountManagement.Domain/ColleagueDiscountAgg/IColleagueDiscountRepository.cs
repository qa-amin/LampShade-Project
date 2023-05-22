using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;

namespace DiscountManagement.Domain.ColleagueDiscountAgg
{
    public interface IColleagueDiscountRepository : IRepository<long , ColleagueDiscount>
    {
       List<ColleagueDiscount> Search(long id);
       ColleagueDiscount GetDetails(long id);
    }
}

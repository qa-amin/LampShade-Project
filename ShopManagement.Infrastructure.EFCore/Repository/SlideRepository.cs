using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class SlideRepository : RepositoryBase<long , Slide>, ISlideRepository
    {
        private readonly ShopManagementDbContext _context;
        public SlideRepository(ShopManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public Slide GetDetails(long id)
        {
            return _context.Slides.Find(id);
        }

        public List<Slide> GetList()
        {
            return _context.Slides.ToList();
        }
    }
}

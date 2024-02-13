using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.Domain
{
	public interface IRepository<TKey, T> where T : class
	{
        Task<T> Get(TKey key);
        Task<List<T>> Get();
        Task<bool> Exists(Expression<Func<T, bool>> exception);
        Task Create(T entity);
        Task SaveChanges();
    }
}

using _0_Framework.Domain;
using AccountManagement.Application.Contracts.Role;

namespace AccountManagement.Domain.RoleAgg
{
    public interface IRoleRepository : IRepository<long, Role>
    {
        Task<List<RoleViewModel>> List();
        Task<EditRole> GetDetails(long id);
    }
}

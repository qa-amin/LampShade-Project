using _0_Framework.Application;
using System.Collections.Generic;

namespace AccountManagement.Application.Contracts.Role
{
    public interface IRoleApplication
    {
        Task<OperationResult> Create(CreateRole command);
        Task<OperationResult> Edit(EditRole command);
        Task<List<RoleViewModel>> List();
        Task<EditRole> GetDetails(long id);
    }
}

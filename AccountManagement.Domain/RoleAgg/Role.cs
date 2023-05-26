using _0_Framework.Domain;
using AccountManagement.Domain.AccountAgg;
using System.Collections.Generic;

namespace AccountManagement.Domain.RoleAgg
{
    public class Role : EntityBase
    {
        public string Name { get; private set; }
        
        public List<Account> Accounts { get; private set; }

        
        
    }
}
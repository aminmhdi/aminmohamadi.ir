using Microsoft.AspNet.Identity;
using MyWeb.DomainClasses.Entities;
using MyWeb.ServiceLayer.Contracts;

namespace MyWeb.ServiceLayer.EFServices.Roles
{
    public class CustomRoleStore : ICustomRoleStore
    {
        private readonly IRoleStore<Role, long> _roleStore;

        public CustomRoleStore(IRoleStore<Role, long> roleStore)
        {
            _roleStore = roleStore;
        }
    }
}

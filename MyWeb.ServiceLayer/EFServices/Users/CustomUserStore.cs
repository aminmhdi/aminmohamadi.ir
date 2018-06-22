using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using MyWeb.DataLayer.Context;
using MyWeb.DomainClasses.Entities;
using MyWeb.ServiceLayer.Contracts;

namespace MyWeb.ServiceLayer.EFServices.Users
{
    public class CustomUserStore : UserStore<User, Role, long, UserLogin, UserRole, UserClaim>, ICustomUserStore
    {
        private readonly DbSet<User> _users;

        public CustomUserStore(MyWebContext dbContext)
            : base(dbContext)
        {
            _users = (DbSet<User>)dbContext.Set<User>();
        }

        public override Task<User> FindByIdAsync(long userId)
        {
            return _users.FindAsync(userId);
        }
    }
}

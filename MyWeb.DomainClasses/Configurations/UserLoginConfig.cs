using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
    public class UserLoginConfig : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginConfig()
        {
            HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
            ToTable("UserLogins");

        }
    }
}

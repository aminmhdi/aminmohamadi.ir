using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
    public class UserRoleConfig : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfig()
        {
            HasKey(r => new { r.UserId, r.RoleId });
            ToTable("UserRole");
        }
    }
}

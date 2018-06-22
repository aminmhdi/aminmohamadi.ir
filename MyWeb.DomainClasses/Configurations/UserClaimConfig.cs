using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
    public class UserClaimConfig : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimConfig()
        {
            ToTable("UserClaims");
        }
    }
}

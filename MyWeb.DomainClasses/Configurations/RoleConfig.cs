using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
    public class RoleConfig : EntityTypeConfiguration<Role>
    {
        public RoleConfig()
        {
            ToTable("Roles");
            Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_RoleName") { IsUnique = true }));
            Property(r => r.RowVersion).IsRowVersion();
            Property(r => r.Permissions).HasColumnType("xml");
            //this.Filter(RoleFilters.ActiveList, a => a.Condition(u => !u.IsBanned));
            Ignore(r => r.XmlPermission);
            HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }
    }
}

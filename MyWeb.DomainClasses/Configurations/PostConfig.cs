using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
  public class PostConfig : EntityTypeConfiguration<Post>
  {
    public PostConfig()
    {
      Property(u => u.Title).IsRequired().HasMaxLength(150);
      Property(u => u.Keyword).IsOptional().HasMaxLength(200);
      Property(u => u.RowVersion).IsRowVersion();
      HasMany(r => r.Comments).WithRequired(r => r.Post).HasForeignKey(r => r.PostId).WillCascadeOnDelete(false);
      HasMany(r => r.Reacts).WithRequired(r => r.Post).HasForeignKey(r => r.PostId).WillCascadeOnDelete(false);
    }
  }
}

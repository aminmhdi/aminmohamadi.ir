using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
  public class CategoryConfig : EntityTypeConfiguration<Category>
  {
    public CategoryConfig()
    {
      Property(u => u.Title).IsRequired().HasMaxLength(50);
      Property(u => u.Description).IsOptional().HasMaxLength(200);
      Property(u => u.RowVersion).IsRowVersion();
      HasMany(r => r.Posts).WithRequired(r => r.Category).HasForeignKey(r => r.CategoryId).WillCascadeOnDelete(false);
    }
  }
}

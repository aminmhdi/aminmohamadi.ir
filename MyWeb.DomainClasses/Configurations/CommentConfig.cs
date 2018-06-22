using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
  public class CommentConfig : EntityTypeConfiguration<Comment>
  {
    public CommentConfig()
    {
      Property(u => u.Body).IsRequired().HasMaxLength(1000);
      Property(u => u.Ip).IsOptional().HasMaxLength(50);
      Property(u => u.RowVersion).IsRowVersion();
    }
  }
}

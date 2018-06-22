using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
  public class PostReactConfig : EntityTypeConfiguration<PostReact>
  {
    public PostReactConfig()
    {
      Property(u => u.RowVersion).IsRowVersion();
    }
  }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.DomainClasses.Configurations
{
  public class WebViewConfig : EntityTypeConfiguration<WebView>
  {
    public WebViewConfig()
    {
      ToTable("WebView");
      Property(r => r.Session).HasColumnAnnotation(
        IndexAnnotation.AnnotationName,
        new IndexAnnotation(
          new IndexAttribute("IX_Session", 1) {IsUnique = true}));

      Property(u => u.Ip).IsRequired().HasMaxLength(50);
      Property(u => u.Session).IsRequired().HasMaxLength(100);
      Property(u => u.BrowserName).IsOptional().HasMaxLength(50);
      Property(u => u.BrowserVersion).IsOptional().HasMaxLength(50);
      Property(u => u.Platfrom).IsOptional().HasMaxLength(50);
      Property(u => u.Device).IsOptional().HasMaxLength(50);
      Property(u => u.MoreInfo).IsOptional().HasMaxLength(150);
      Property(u => u.UserId).IsOptional();
      Property(u => u.RowVersion).IsRowVersion();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.DomainClasses.Entities
{
  public class WebView
  {
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string Ip { get; set; }
    public string Session { get; set; }
    public string BrowserName { get; set; }
    public string BrowserVersion { get; set; }
    public string Platfrom { get; set; }
    public string Device { get; set; }
    public string MoreInfo { get; set; }

    public long? UserId { get; set; }

    public byte[] RowVersion { get; set; }
  }
}

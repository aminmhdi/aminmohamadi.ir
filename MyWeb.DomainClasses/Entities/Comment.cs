using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.DomainClasses.Entities
{
  public class Comment : Base
  {
    public string Body { get; set; }
    public string Ip { get; set; }
    public virtual Post Post { get; set; }
    public virtual long PostId { get; set; }
  }
}

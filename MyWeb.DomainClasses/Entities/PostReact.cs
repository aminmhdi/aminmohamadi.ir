using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.DomainClasses.Entities
{
  public class PostReact
  {
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; }

    public virtual Post Post { get; set; }
    public virtual long PostId { get; set; }

    public virtual User User { get; set; }
    public virtual long UserId { get; set; }

    public virtual bool Like { get; set; }

    public byte[] RowVersion { get; set; }
  }
}

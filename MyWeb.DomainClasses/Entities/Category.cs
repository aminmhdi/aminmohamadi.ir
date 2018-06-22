using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.DomainClasses.Entities
{
  public class Category : Base
  {
    public string Title { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Post> Posts { get; set; }

  }
}

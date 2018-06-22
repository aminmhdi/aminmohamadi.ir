using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.DomainClasses.Entities
{
  public class Post : Base
  {
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Body { get; set; }
    public string Keyword { get; set; }
    public virtual Category Category { get; set; }
    public long CategoryId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<PostReact> Reacts { get; set; }
  }
}

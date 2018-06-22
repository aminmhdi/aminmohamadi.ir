using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWeb.DomainClasses.Entities
{
  public class Base
  {

    #region Ctor

    public Base()
    {
      CreatedOn = DateTime.Now;
      IsDelete = false;
    }

    #endregion

    #region Properties

    public long Id { get; set; }

    public byte[] RowVersion { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }
    #endregion

    #region Navigation
    [ForeignKey("CreatorId")]
    public User Creator { get; set; }

    public long CreatorId { get; set; }

    [ForeignKey("ModifierId")]
    public User Modifier { get; set; }
    public long? ModifierId { get; set; }

    #endregion

  }
}

using System.Collections.Generic;

namespace MyWeb.ServiceLayer
{
  public class PermissionRecord
  {
    public string RoleName { get; set; }
    public bool IsDefaultForRegister { get; set; }
    public IEnumerable<PermissionModel> Permissions { get; set; }
  }
}

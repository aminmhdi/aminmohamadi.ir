using System.Collections.Generic;
using System.Xml.Linq;

namespace MyWeb.ServiceLayer.Contracts
{
    public interface IPermissionService
    {
        XElement GetPermissionsAsXml(params string[] permissionNames);
        IList<string> GetUserPermissionsAsList(XElement permissionsAsXml);
        IList<string> GetUserPermissionsAsList(IList<XElement> permissionsAsXmls);

    }
}

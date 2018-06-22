using System.Net;
using System.Web;

namespace MyWeb.Utility
{
  public static class Network
  {
    public static string GetSessionId()
    {
      return HttpContext.Current.Session.SessionID;
    }
    public static string GetClientIp()
    {
      var ipAddress = string.Empty;

      if (HttpContext.Current.Request.UserHostAddress == null)
        return ipAddress;

      var strHostName = HttpContext.Current.Request.UserHostAddress;
      ipAddress = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();

      return ipAddress;
    }
  }
}

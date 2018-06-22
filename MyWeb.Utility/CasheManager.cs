using System.Web;

namespace MyWeb.Utility
{
    public static class CasheManager
    {
        public static void InvalidateCache(this HttpContextBase httpContext, string key)
        {
            httpContext.Cache.Remove(key);
        }
    }
}

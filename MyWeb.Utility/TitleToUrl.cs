using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyWeb.Utility
{
    public static class TitleToUrl
    {
        public static string ToUrl(this string value)
        {
            return Regex.Replace(value, @"[^A-Za-z0-9\u0600-\u06FF_\.~]+", "-");
        }
    }
}

using System;
using System.Net;

namespace MyWeb.Utility
{
    public class Webclient
    {
        private static Webclient _instance;
        private static readonly object SyncRoot = new Object();

        public static Webclient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new Webclient();
                    }
                }

                return _instance;
            }
        }

        public string GetUrl(string url)
        {
            try
            {
                if (!CheckUrl(url))
                    return "Url is not correct";

                var client = new WebClient();
                //client.Credentials.GetCredential();
                var value = client.DownloadString(url);
                return value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private bool CheckUrl(string url)
        {
            Uri uriResult;
            var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;

            return result;
        }
    }
}

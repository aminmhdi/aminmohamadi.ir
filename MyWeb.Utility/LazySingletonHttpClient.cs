using System;
using System.Net.Http;
using System.Threading;

namespace MyWeb.Utility
{
    public sealed class LazySingletonHttpClient: HttpClient
    {
        private static readonly Lazy<LazySingletonHttpClient> _instance =
            new Lazy<LazySingletonHttpClient>(() => new LazySingletonHttpClient(), LazyThreadSafetyMode.ExecutionAndPublication);

        private LazySingletonHttpClient()
        {
        }

        public static LazySingletonHttpClient Instance
        {
            get { return _instance.Value; }
        }
    }
}

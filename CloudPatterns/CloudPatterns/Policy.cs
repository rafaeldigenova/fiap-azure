using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    static class Poly
    {
        public static void PolicyMain(string[] args)
        {
            var policyResult = Policy
                .Handle<HttpRequestException>()
                .RetryAsync()
                .ExecuteAndCapture(() => CallApi());
        }

        private static bool CallApi()
        {
            return true;
        }
    }
}

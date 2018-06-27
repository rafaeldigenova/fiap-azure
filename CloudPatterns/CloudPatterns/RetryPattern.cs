using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    static class RetryPattern
    {
        static void Execute(string[] args)
        {
            int retryCount = 5;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {

                    var response = CallApi();
                    if (response) break;
                }
                catch (Exception ex)
                {
                    if (ex.HResult == 502) throw; //erro não transiente
                    if (ex.InnerException != null && i + 1 == retryCount)
                    {
                        throw;
                    }
                }
            }
        }

        private static bool CallApi()
        {
            return true;
        }
    }
}

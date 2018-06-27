using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheAsidePattern.Execute(args);
        }
    }
}

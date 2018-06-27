using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    static class RedisCache
    {

        public static void Execute(string[] args)
        {
            var connection = ConnectionMultiplexer.Connect("mbafiapredis.redis.cache.windows.net:6380,password=0HhNtNatKRkFitAWl+Ou8uhcwWK+Ie+5ryac6F5bn6s=,ssl=True,abortConnect=false");
            var db = connection.GetDatabase();

            for (int i = 0; i < 100; i++)
            {
                db.StringSet(i.ToString(), Guid.NewGuid().ToString());
            }

            Console.WriteLine("Digite o id");
            var id = Console.ReadLine();
            var item = db.StringGet(id.ToString());

            if(!string.IsNullOrWhiteSpace(item))
            {
                Console.WriteLine(item);
            }else
            {
                Console.WriteLine($"Não existe valor para o id {id}");
            }

            Console.Read();

        }
    }
}

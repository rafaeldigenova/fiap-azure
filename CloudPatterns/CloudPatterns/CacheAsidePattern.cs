using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    static class CacheAsidePattern
    {
        static List<GuidEntity> cache
            = new List<GuidEntity>();

        public static void Execute(string[] args)
        {
            LoadCache();

            Console.WriteLine("digite o id");
            int id = 0;
            int.TryParse(Console.ReadLine(), out id);

            if(cache.Any(x => x.RowKey == id.ToString()))
            {
                Console.WriteLine($"id: {id} - valor: {cache.First(x => x.RowKey == id.ToString()).Guid}");
            }
            else
            {
                SaveNewValue(id);
            }
            Console.Read();
        }

        static void LoadCache()
        {
            var values = GetValues();

            if (!values.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    SaveNewValue(i);
                }
            }else
            {
                cache.AddRange(values);
            }
        }

        static IEnumerable<GuidEntity> GetValues()
        {
            CloudTable tableReference = GetTableReference();

            return tableReference.ExecuteQuery(new TableQuery<GuidEntity>());
        }

        static void SaveNewValue(int id)
        {
            CloudTable tableReference = GetTableReference();

            var guidEntity = new GuidEntity(id)
            {
                Guid = Guid.NewGuid().ToString()
            };

            var operation = TableOperation.Insert(guidEntity);

            tableReference.Execute(operation);

            cache.Add(guidEntity);
        }

        private static CloudTable GetTableReference()
        {
            var acc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mbafiapstorage;AccountKey=uGB5fWizvpeuJUvruCGRwtZ2EjpwlN1KqeSwexwOFz7EyXmXLaTl3BnepVesNx409vkf08gCyfKBJULtUPZkhQ==;EndpointSuffix=core.windows.net");

            var client = acc.CreateCloudTableClient();

            var tableReference = client.GetTableReference("guids");
            tableReference.CreateIfNotExists();
            return tableReference;
        }
    }

    public class GuidEntity : TableEntity
    {
        public GuidEntity(int id)
        {
            this.PartitionKey = "rm330099";
            this.RowKey = id.ToString();
        }

        public GuidEntity() { }

        public string Guid{ get; set; }
    }
}

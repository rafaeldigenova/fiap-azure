using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.ComponentModel.DataAnnotations;

namespace ExemploTextAnalytics
{
    class CustomDocument
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        [IsSortable]
        [IsRetrievable(true)]
        [JsonProperty("id")]
        public string Id { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsRetrievable(true)]
        [IsSearchable]
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsRetrievable(true)]
        [IsSearchable]
        [JsonProperty("email")]
        public string Email { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsRetrievable(true)]
        [IsSearchable]
        [JsonProperty("idade")]
        public int Idade { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsRetrievable(true)]
        [IsFacetable]
        [JsonProperty("cidade")]
        public string Cidade { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsRetrievable(true)]
        [JsonProperty("endereco")]
        public string Endereco { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //DoCognitiveAnalysis();
            //DoAzureSeachInsert();
            DoAzureSearchSearch();
        }
    
        static void DoAzureSearchSearch()
        {
            var client = new SearchServiceClient("thnetazsearch", new SearchCredentials("6E74D968A34AD1E6A47C9EEAD75ABD09"));

            var index = client.Indexes.GetClient("mbafiap");

            Console.WriteLine("Digite um termo para a busca");

            var termo = Console.ReadLine();
            var response = index.Documents.Search<CustomDocument>(termo, new SearchParameters
            {
                IncludeTotalResultCount = true,
                OrderBy = new List<string> { "nome" },
                Filter = "idade gt 27"
            });

            Console.WriteLine($"{response.Count} documentos encontrados");
            foreach (var r in response.Results)
            {
                Console.WriteLine($"{r.Document.Nome} {r.Document.Email} - {r.Document.Cidade}");
            }

            Console.Read();
        }

        static void DoAzureSeach()
        {
            //var endpoint = "https://thnetazsearch.search.windows.net";

            var client = new SearchServiceClient("thnetazsearch", new SearchCredentials("6E74D968A34AD1E6A47C9EEAD75ABD09"));

            var index = client.Indexes.GetClient("mbafiap");
            var document = new CustomDocument()
            {
                Id = "rm39611",
                Cidade = "Mauá",
                Email = "edvaldo.farias@gmail.com",
                Endereco = "rua alcindo dian, 50",
                Idade = 28,
                Nome = "Edvaldo Farias"
            };

            var batch = IndexBatch.MergeOrUpload(new List<CustomDocument>()
            {
                document
            });

            index.Documents.Index(batch);
        }
        

        static void DoCognitiveAnalysis()
        {
            Console.WriteLine("Como foi sua experiência em nossa loja?");
            string opiniao = Console.ReadLine();

            var doc = new
            {
                documents = new[]
                {
                    new {
                        id = 1,
                        text = opiniao,
                        //language = "en"
                    }
                }
            };

            var json = JsonConvert.SerializeObject(doc);

            var key = "514ae7b737e24f87a42d472a136e5760";

            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            byte[] content = UTF8Encoding.UTF8.GetBytes(json);

            using (var payload = new ByteArrayContent(content))
            {
                payload.Headers.ContentType
                    = new MediaTypeHeaderValue("application/json");

                var endpoint = "https://brazilsouth.api.cognitive.microsoft.com/text/analytics/v2.0/languages";

                HttpResponseMessage response = http.PostAsync(endpoint, payload).Result;

                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Console.Read();
            }
        }
    }
}

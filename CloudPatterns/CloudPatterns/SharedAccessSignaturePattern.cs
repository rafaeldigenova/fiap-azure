using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    static class SharedAccessSignaturePattern
    {
        static void Execute(string[] args)
        {
            //"DefaultEndpointsProtocol=https;AccountName=mbafiapstorage;AccountKey=uGB5fWizvpeuJUvruCGRwtZ2EjpwlN1KqeSwexwOFz7EyXmXLaTl3BnepVesNx409vkf08gCyfKBJULtUPZkhQ==;EndpointSuffix=core.windows.net"
            var acc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mbafiapstorage;AccountKey=uGB5fWizvpeuJUvruCGRwtZ2EjpwlN1KqeSwexwOFz7EyXmXLaTl3BnepVesNx409vkf08gCyfKBJULtUPZkhQ==;EndpointSuffix=core.windows.net");

            var client = acc.CreateCloudBlobClient();

            var container = client.GetContainerReference("alunos");
            var blob = container.GetBlockBlobReference("rm330099.txt");

            blob.UploadText("Lorem ipsum");

            var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(3),
                SharedAccessStartTime = DateTimeOffset.UtcNow,
                Permissions= SharedAccessBlobPermissions.Add | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read |
                    SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Create | SharedAccessBlobPermissions.Delete
            });

            Console.WriteLine(blob.Uri + sas);
            Console.Read();

        }
    }
}

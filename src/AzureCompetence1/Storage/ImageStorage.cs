using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureCompetence1.Storage
{
    public interface IImageStorage
    {
        byte[] GetImage(string imageName, string imageSize);
        void Insert(string imageName, byte[] incomingData);
        IEnumerable<string> Images { get; }
    }

    public class ImageEntity : TableEntity
    {
        public ImageEntity()
        {

        }

        public ImageEntity(string name)
        {
            PartitionKey = name;
            RowKey = name;
            Name = name;
        }

        public string Name { get; set; }
    }

    public class ImageStorage : IImageStorage
    {
        private CloudStorageAccount blobStorageAccount;

        public ImageStorage(string blobConnectionString)
        {
            blobStorageAccount = CloudStorageAccount.Parse(blobConnectionString);
        }

        public IEnumerable<string> Images
        {
            get
            {
                var blobClient = blobStorageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("images");
                if (container.Exists())
                {
                    foreach (var item in container.ListBlobs(null, false).OfType<CloudBlobDirectory>())
                    {                        
                        yield return item.Prefix.TrimEnd('/');
                    }
                }
            }
        }

        public byte[] GetImage(string imageName, string imageSize)
        {
            var blobClient = blobStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");
            var directory = container.GetDirectoryReference(imageName.ToLower());            
            var blockBlob = directory.GetBlockBlobReference(imageSize.ToLower());
            if (!blockBlob.Exists())
                return null;
            var memoryStream = new MemoryStream();
            blockBlob.DownloadToStream(memoryStream);
            return memoryStream.ToArray();
        }

        public void Insert(string imageName, byte[] image)
        {
            var blobClient = blobStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");
            container.CreateIfNotExists();
            var blockBlob = container.GetBlockBlobReference($"{imageName.ToLower()}\\regular");
            blockBlob.UploadFromByteArray(image, 0, image.Length);
        }
    }
}

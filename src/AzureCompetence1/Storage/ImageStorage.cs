using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureCompetence1.Storage
{
    public interface IImageStorage
    {
        void Insert(string imageName);
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
        private CloudStorageAccount tableStorageAccount;

        public ImageStorage(string blobConnectionString, string tableConnectionString)
        {
            blobStorageAccount = CloudStorageAccount.Parse(blobConnectionString);
            tableStorageAccount = CloudStorageAccount.Parse(tableConnectionString);
        }

        public IEnumerable<string> Images
        {
            get
            {
                var tableClient = tableStorageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("images");
                // Create the table if it doesn't exist.
                table.CreateIfNotExists();
                
                return table.ExecuteQuery(new TableQuery<ImageEntity>()).Select(i => i.Name).ToArray();                
            }
        }

        public void Insert(string imageName)
        {
            var tableClient = tableStorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("images");
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            var img = new ImageEntity(imageName);
         
            // Create the TableOperation object that inserts the customer entity.
            var insertOperation = TableOperation.Insert(img);
            // Execute the insert operation.
            table.Execute(insertOperation);
        }
    }
}

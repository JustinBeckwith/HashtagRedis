using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerfPage.Storage
{
    public class DummyType : TableEntity
    {
        public string data;

        public DummyType(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public DummyType() { }
    }

    public class StorageController : PerfPage.MyHub.Harness
    {
        private const string TableName = "TableStore";
        private CloudTable table;

        public StorageController()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            this.table = tableClient.GetTableReference(StorageController.TableName);
            this.table.CreateIfNotExists();
        }

        public override string Name
        {
            get 
            {
                return StorageController.TableName;
            }
        }

        public override void Create(string data) 
        {
            var dummy = new DummyType() { data = data };
            TableOperation insertOperation = TableOperation.Insert(dummy);
            this.table.Execute(insertOperation);
        }

        public override void Read(string data)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<DummyType>(data, data);
            TableResult retrievedResult = table.Execute(retrieveOperation);
            DummyType deleteEntity = (DummyType)retrievedResult.Result;
        }

        public override void Delete(string data)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<DummyType>(data, data);
            TableResult retrievedResult = table.Execute(retrieveOperation);
            DummyType deleteEntity = (DummyType)retrievedResult.Result;

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                table.Execute(deleteOperation);
            }
        }
    }
}
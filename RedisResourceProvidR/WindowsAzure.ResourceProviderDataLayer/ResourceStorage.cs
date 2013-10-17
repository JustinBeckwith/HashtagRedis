using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace WindowsAzure.ResourceProviderDataLayer
{
    public class ResourceStorage : IResourceStorage
    {
        private const string SubscriptionTableName = "Resources";

        private CloudStorageAccount StorageAccount { get; set; }
        private CloudTableClient TableClient { get; set; }
        private CloudTable Table { get; set; }

        public ResourceStorage(CloudStorageAccount cloudStorageAccount)
        {
            StorageAccount = cloudStorageAccount;
            InitializeClient();
            EnsureTable();
        }

        public IEnumerable<ResourceEntity> GetResources(string subscriptionId, string cloudServiceName)
        {
            string partitionKey = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", cloudServiceName, subscriptionId);

            TableQuery<ResourceEntity> query = new TableQuery<ResourceEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return Table.ExecuteQuery(query);
        }

        public ResourceEntity GetResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName)
        {
            // TODO: Share this with the other class
            string partitionKey = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", cloudServiceName, subscriptionId);
            string rowKey = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", resourceType, resourceName);

            TableOperation retrieveOperation = TableOperation.Retrieve<ResourceEntity>(partitionKey, rowKey);

            try
            {
                TableResult retrievedResource = Table.Execute(retrieveOperation);
                return retrievedResource.Result != null ? retrievedResource.Result as ResourceEntity : null;
            }
            catch (WebException ex)
            {
                HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
                if ((httpWebResponse != null) && (httpWebResponse.StatusCode != HttpStatusCode.NotFound))
                {
                    return null;
                }

                throw;
            }
        }

        public void AddOrUpdateResource(ResourceEntity resourceEntity)
        {
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(resourceEntity);
            Table.Execute(insertOrReplaceOperation);
        }

        public void DeleteResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName)
        {
            ResourceEntity resourceEntity = GetResource(subscriptionId, cloudServiceName, resourceType, resourceName);
            if (resourceEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(resourceEntity);
                Table.Execute(deleteOperation);
            }
        }

        private void InitializeClient()
        {
            TableClient = StorageAccount.CreateCloudTableClient();
            Table = TableClient.GetTableReference(SubscriptionTableName);
        }

        private void EnsureTable()
        {            
            Table.CreateIfNotExists();
        }
    }
}

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace WindowsAzure.ResourceProviderDataLayer
{
    public class SubscriptionStorage : ISubscriptionStorage
    {
        private const string SubscriptionTableName = "Subscriptions";

        private CloudStorageAccount StorageAccount { get; set; }
        private CloudTableClient TableClient { get; set; }
        private CloudTable Table { get; set; }

        public SubscriptionStorage(CloudStorageAccount cloudStorageAccount)
        {
            StorageAccount = cloudStorageAccount;
            InitializeClient();
            EnsureTable();
        }

        public SubscriptionEntity GetSubscriptionById(string subscriptionId)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<SubscriptionEntity>(subscriptionId, subscriptionId);

            try
            {
                TableResult retrievedSubscription = Table.Execute(retrieveOperation);
                return retrievedSubscription.Result != null ? retrievedSubscription.Result as SubscriptionEntity : null;
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

        public void AddOrUpdateSubscription(SubscriptionEntity subscriptionEntity)
        {
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(subscriptionEntity);
            Table.Execute(insertOrReplaceOperation);
        }

        public void DeleteSubscriptionById(string subscriptionId)
        {
            SubscriptionEntity subscriptionEntity = GetSubscriptionById(subscriptionId);
            if (subscriptionEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(subscriptionEntity);
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

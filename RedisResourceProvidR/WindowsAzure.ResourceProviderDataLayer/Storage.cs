using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace WindowsAzure.ResourceProviderDataLayer
{
    public class Storage
    {
        private string storageAccountName;
        private string storageAccountKey;

        private object cloudStorageAccountSyncLock = new object();
        private volatile CloudStorageAccount storageAccount;
        public CloudStorageAccount StorageAccount
        {
            get
            {
                if (storageAccount == null)
                {
                    lock (cloudStorageAccountSyncLock)
                    {
                        if (storageAccount == null)
                        {
                            StorageCredentials storageCredentials = new StorageCredentials(storageAccountName, storageAccountKey);
                            storageAccount = new CloudStorageAccount(storageCredentials, true);
                        }
                    }
                }

                return storageAccount;
            }
        }
        public ISubscriptionStorage SubscriptionStorage { get; private set; }
        public IResourceStorage ResourceStorage { get; private set; }
        public Storage(string azureStorageAccountName, string azureStorageAccountKey)
        {
            // Remember storage connection string
            storageAccountName = azureStorageAccountName;
            storageAccountKey = azureStorageAccountKey;

            // Create all needed storage accessors
            SubscriptionStorage = new SubscriptionStorage(StorageAccount);
            ResourceStorage = new ResourceStorage(StorageAccount);
        }
    }
}

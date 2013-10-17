using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace WindowsAzure.ResourceProviderDataLayer.Entities
{
    public class ResourceEntity : TableEntity
    {
        public ResourceEntity() : base()
        {
        }

        public ResourceEntity(string subscriptionId, string cloudServiceName, string resourceType, string resourceName) : base()
        {
            PartitionKey = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", cloudServiceName, subscriptionId);
            RowKey = String.Format(CultureInfo.InvariantCulture, "{0}-{1}", resourceType, resourceName);
        }

        public string SubscriptionId { get; set; }

        public string CloudServiceName { get; set; }

        public string CloudServiceGeoRegion { get; set; }

        public string ResourceETag { get; set; }

       // public XmlNode[] IntrinsicSettings { get; set; }

        public string Name { get; set; }

        public string OutputItems { get; set; }

        public string Plan { get; set; }
        public string PromotionCode { get; set; }

        public string SchemaVersion { get; set; }

        public string State { get; set; }

        public string SubState { get; set; }

        public string Type { get; set; }

        public string Label { get; set; }

        public string BillingETag { get; set; }

        public string LastOperation { get; set; }

        public int LastOperationHttpCode { get; set; }

        public string LastOperationMessage { get; set; }

        public string LastOperationResult { get; set; }
    }
}
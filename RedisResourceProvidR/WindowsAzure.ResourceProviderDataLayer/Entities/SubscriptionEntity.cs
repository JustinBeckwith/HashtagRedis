using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace WindowsAzure.ResourceProviderDataLayer.Entities
{
    public class SubscriptionEntity : TableEntity
    {
        public SubscriptionEntity() : base()
        {
        }

        public SubscriptionEntity(string subscriptionId) : base(subscriptionId, subscriptionId)
        {
            PartitionKey = subscriptionId;
            RowKey = subscriptionId;
        }

        public string State { get; set; }

        public DateTimeOffset Created { get; set; }

        public string PreviousState { get; set; }

        public string LastOperation { get; set; }

        public string LastOperationId { get; set; }

        public DateTimeOffset LastOperationTime { get; set; }

        public string LastEventId { get; set; }

        public string Properties { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Http;
using WindowsAzure.ResourceProviderContracts;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace ResourceProvidR.Controllers
{
    public class EventsController : ApiController
    {
        //
        // POST /subscriptions/{subscriptionId}/Events
        //
        [HttpPost]
        public void HandleSubscriptionNotifications(Guid subscriptionId, EntityEvent entityEvent)
        {
            string entityId = GetEntityId(entityEvent);
            SubscriptionEntity susbcriptionEntityOnFile = WebApiApplication.Storage.SubscriptionStorage.GetSubscriptionById(entityId);

            // Only process events we have not already processed - Notification events can be sent multiple times
            if ((susbcriptionEntityOnFile == null) || (String.CompareOrdinal(entityEvent.EventId, susbcriptionEntityOnFile.LastEventId) != 0))
            {
                switch (entityEvent.EntityState)
                {
                    case EntityState.Deleted:
                        WebApiApplication.Storage.SubscriptionStorage.DeleteSubscriptionById(entityId);
                        break;

                    case EntityState.Disabled:
                    case EntityState.Enabled:
                    case EntityState.Registered:
                    case EntityState.Unregistered:
                    case EntityState.Updated:
                        SubscriptionEntity newSubscriptionEntity = GetSubscriptionEntryForOperation(entityEvent, susbcriptionEntityOnFile);
                        WebApiApplication.Storage.SubscriptionStorage.AddOrUpdateSubscription(newSubscriptionEntity);
                        break;

                    //case EntityState.Migrated:
                    //    break;

                    default:
                        // TODO: BAD REQUEST ?
                        break;
                }
            }
        }

        private SubscriptionEntity GetSubscriptionEntryForOperation(EntityEvent entityEvent, SubscriptionEntity subscriptionCurrentlyOnFile)
        {
            string entityId = GetEntityId(entityEvent);
            SubscriptionEntity subscriptionEntryToReturn = subscriptionCurrentlyOnFile;
            if (subscriptionEntryToReturn == null)
            {
                // Create a new entity, based on the current event
                subscriptionEntryToReturn = new SubscriptionEntity(entityId);

                // Populate fields specificcally for entities we are seeing for the first time
                subscriptionEntryToReturn.PreviousState = null;
            }
            else
            {
                // Update existing entity when we have seen it before
                subscriptionEntryToReturn.PreviousState = subscriptionCurrentlyOnFile.State;
            }

            // Common fields
            subscriptionEntryToReturn.State = entityEvent.EntityState.ToString();
            subscriptionEntryToReturn.Created = new DateTimeOffset(entityEvent.EntityId.Created);
            subscriptionEntryToReturn.LastOperation = entityEvent.EntityState.ToString();
            subscriptionEntryToReturn.LastOperationId = entityEvent.OperationId;
            subscriptionEntryToReturn.LastOperationTime = DateTimeOffset.UtcNow;
            subscriptionEntryToReturn.LastEventId = entityEvent.EventId;
            subscriptionEntryToReturn.Properties = SerializeProperties(entityEvent.Properties);

            // CONSIDER: Promoting common user properties as columns in the table

            return subscriptionEntryToReturn;
        }

        private string SerializeProperties(EntityPropertyCollection entityProperties)
        {
            List<KeyValuePair<string, string>> properties = new List<KeyValuePair<string, string>>();
            foreach (EntityProperty property in entityProperties)
            {
                properties.Add(new KeyValuePair<string,string>(property.PropertyName, property.PropertyValue));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(properties);
        }
        private static string GetEntityId(EntityEvent entityEvent)
        {
            return entityEvent.EntityId.Id.ToLowerInvariant();
        }
    }
}
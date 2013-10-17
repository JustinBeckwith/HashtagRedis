using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace WindowsAzure.ResourceProviderDataLayer
{
    public interface ISubscriptionStorage
    {
        SubscriptionEntity GetSubscriptionById(string subscriptionId);
        void AddOrUpdateSubscription(SubscriptionEntity subscriptionEntity);
        void DeleteSubscriptionById(string subscriptionId);
    }
}

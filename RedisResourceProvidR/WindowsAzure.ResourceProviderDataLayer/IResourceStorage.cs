using System.Collections.Generic;
using WindowsAzure.ResourceProviderDataLayer.Entities;

namespace WindowsAzure.ResourceProviderDataLayer
{
    public interface IResourceStorage
    {
        IEnumerable<ResourceEntity> GetResources(string subscriptionId, string cloudServiceName);
        ResourceEntity GetResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName);
        void AddOrUpdateResource(ResourceEntity resourceEntity);
        void DeleteResource(string subscriptionId, string cloudServiceName, string resourceType, string resourceName);
    }
}

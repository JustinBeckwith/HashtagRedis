namespace WindowsAzure.ResourceProviderContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// List of resources returned by the resource provider.
    /// </summary>
    [CollectionDataContract(Name = "Resources", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class ResourceOutputCollection : List<ResourceOutput>
    {
        public ResourceOutputCollection()
        {
        }

        public ResourceOutputCollection(IEnumerable<ResourceOutput> resources) : base(resources)
        {
        }
    }
}

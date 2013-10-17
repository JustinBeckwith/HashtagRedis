namespace WindowsAzure.ResourceProviderContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// List of usage meters returned by the resource provider.
    /// </summary>
    [CollectionDataContract(Name = "UsageMeters", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class UsageMeterCollection : List<UsageMeter>
    {
        public UsageMeterCollection()
        {
        }

        public UsageMeterCollection(IEnumerable<UsageMeter> meters) : base(meters)
        {
        }
    }
}

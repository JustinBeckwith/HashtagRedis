namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Class with the result of a resource name avalability query.
    /// </summary>
    [DataContract(Name = "ResourceNameAvailabilityResponse", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class ResourceNameAvailability : IExtensibleDataObject
    {
        /// <summary>
        /// True if the name is available; false otherwise.
        /// </summary>
        [DataMember(Order = 1)]
        public bool IsAvailable { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

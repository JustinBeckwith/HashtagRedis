namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Cloud service information returned by the resource provider.
    /// </summary>
    [DataContract(Name = "CloudService", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class CloudServiceOutput : IExtensibleDataObject
    {
        /// <summary>
        /// Geo region of the cloud service.
        /// </summary>
        [DataMember(Order = 1)]
        public string GeoRegion { get; set; }

        /// <summary>
        /// The resources of the cloud service.
        /// </summary>
        [DataMember(Order = 2)]
        public ResourceOutputCollection Resources { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

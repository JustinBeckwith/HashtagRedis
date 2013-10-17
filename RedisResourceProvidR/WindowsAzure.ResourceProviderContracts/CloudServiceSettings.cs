namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Settings of the cloud service that are sent in the resource-level operations.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class CloudServiceSettings : IExtensibleDataObject
    {
        /// <summary>
        /// The geo region of the cloud service.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string GeoRegion { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

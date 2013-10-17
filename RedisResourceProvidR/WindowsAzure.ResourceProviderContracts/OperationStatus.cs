namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Status about an operation.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class OperationStatus : IExtensibleDataObject
    {
        /// <summary>
        /// Error information for an unhealthy resource.
        /// </summary>
        [DataMember(Order = 1)]
        public ErrorData Error { get; set; }

        /// <summary>
        /// Operation result. 
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public OperationResult Result { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Output item.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class OutputItem : IExtensibleDataObject
    {
        /// <summary>
        /// The key of the output item
        /// </summary>
        [DataMember(Order = 1)]
        public string Key { get; set; }

        /// <summary>
        /// The value of the output item
        /// </summary>
        [DataMember(Order = 2)]
        public string Value { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

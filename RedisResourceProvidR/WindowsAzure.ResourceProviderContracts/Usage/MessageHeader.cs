namespace WindowsAzure.ResourceProviderContracts.Usage
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Message header of the notification messages.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class MessageHeader : IExtensibleDataObject
    {
        /// <summary>
        /// Schema version, which should be aligned with RPC version.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false, IsRequired = true)]
        public string Version { get; set; }

        /// <summary>
        /// Job insertion time.
        /// </summary>
        [DataMember(Order = 2, EmitDefaultValue = false, IsRequired = true)]
        public DateTime InsertionTime { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

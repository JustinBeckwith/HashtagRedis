namespace WindowsAzure.ResourceProviderContracts.Usage
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Notification message schema for resource providers to report usages via queue & table infrastructure.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class ReportUsageEvents : IExtensibleDataObject
    {
        /// <summary>
        /// Message header that includes meta information about this message, like version and insertion time.
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false, IsRequired = true)]
        public MessageHeader Header { get; set; }

        /// <summary>
        /// JobId, which should be the PartitionKey of the batch of usage events in resource provider's usage event table.
        /// </summary>
        [DataMember(Order = 2, EmitDefaultValue = false, IsRequired = true)]
        public string JobId { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

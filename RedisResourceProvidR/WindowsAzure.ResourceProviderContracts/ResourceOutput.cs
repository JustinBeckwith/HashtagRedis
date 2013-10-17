namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Resource information returned by the resource provider.
    /// </summary>
    [DataContract(Name = "Resource", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class ResourceOutput : IExtensibleDataObject
    {
        /// <summary>
        /// Cloud service settings. Resource providers are expected to return this data unchanged, from ResourceInput.
        /// </summary>
        [DataMember(Order = 1)]
        public CloudServiceSettings CloudServiceSettings { get; set; }

        /// <summary>
        /// Resource Etag.
        /// </summary>
        [DataMember(Order = 2)]
        public string ETag { get; set; }

        /// <summary>
        /// Intrinsic settings of the resource.
        /// </summary>
        [DataMember(Order = 3)]
        public XmlNode[] IntrinsicSettings { get; set; }

        /// <summary>
        /// The name of the resource. It is unique within a cloud service.
        /// </summary>
        [DataMember(Order = 4)]
        public string Name { get; set; }

        /// <summary>
        /// Status about the last operation on this resource.
        /// </summary>
        [DataMember(Order = 5)]
        public OperationStatus OperationStatus { get; set; }

        /// <summary>
        /// Resource output items. Can be null.
        /// </summary>
        [DataMember(Order = 6)]
        public OutputItemCollection OutputItems { get; set; }

        /// <summary>
        /// Resource plan.
        /// </summary>
        [DataMember(Order = 7)]
        public string Plan { get; set; }

        /// <summary>
        /// The resource promotion code. The resource provider is not required to return this field.
        /// </summary>
        [DataMember(Order = 8)]
        public string PromotionCode { get; set; }

        /// <summary>
        /// Schema version of the intrinsic settings.
        /// </summary>
        [DataMember(Order = 9)]
        public string SchemaVersion { get; set; }

        /// <summary>
        /// State of the resource.
        /// </summary>
        [DataMember(Order = 10)]
        public string State { get; set; }

        /// <summary>
        /// Sub-state of the resource, resource provider defined.
        /// </summary>
        [DataMember(Order = 11)]
        public string SubState { get; set; }

        /// <summary>
        /// Type of the resource.
        /// </summary>
        [DataMember(Order = 12)]
        public string Type { get; set; }

        /// <summary>
        /// The usage meters of the resource. Can be null.
        /// </summary>
        [DataMember(Order = 13)]
        public UsageMeterCollection UsageMeters { get; set; }

        /// <summary>
        /// Resource label.
        /// </summary>
        [DataMember(Order = 14)]
        public string Label { get; set; }

        /// <summary>
        /// Billing ETag - Resource providers can use this identifier to emit usage for the resource.
        /// </summary>
        [DataMember(Order = 15)]
        public string BillingETag { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

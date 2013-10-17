namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Resource information sent to the resource provider for the resource-level operations.
    /// </summary>
    [DataContract(Name = "Resource", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class ResourceInput : IExtensibleDataObject
    {
        /// <summary>
        /// Cloud service settings
        /// </summary>
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public CloudServiceSettings CloudServiceSettings { get; set; }

        /// <summary>
        /// Resource ETag
        /// </summary>
        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string ETag { get; set; }

        /// <summary>
        /// Intrinsic settings of the resource
        /// </summary>
        [DataMember(Order = 3, EmitDefaultValue = false)]
        public XmlNode[] IntrinsicSettings { get; set; }

        /// <summary>
        /// Resource plan.
        /// </summary>
        [DataMember(Order = 4, EmitDefaultValue = false)]
        public string Plan { get; set; }

        /// <summary>
        /// Promotion code
        /// </summary>
        [DataMember(Order = 5, EmitDefaultValue = false)]
        public string PromotionCode { get; set; }

        /// <summary>
        /// Schema version of intrinsic settings
        /// </summary>
        [DataMember(Order = 6, EmitDefaultValue = false)]
        public string SchemaVersion { get; set; }

        /// <summary>
        /// Type of the resource
        /// </summary>
        [DataMember(Order = 7, EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        [DataMember(Order = 8, EmitDefaultValue = false)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the BillingETag
        /// </summary>
        [DataMember(Order = 9, EmitDefaultValue = false)]
        public string BillingETag { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

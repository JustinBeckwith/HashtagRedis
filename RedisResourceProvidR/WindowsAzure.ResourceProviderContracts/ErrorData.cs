namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Error information about a failed operation.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/windowsazure", Name = "Error")]
    public class ErrorData : IExtensibleDataObject
    {
        /// <summary>
        /// The HTTP error code.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int HttpCode { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        /// <summary>
        /// Non-localized error code defined by the resource provider.
        /// </summary>
        [DataMember(Order = 3)]
        public string ExtendedCode { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

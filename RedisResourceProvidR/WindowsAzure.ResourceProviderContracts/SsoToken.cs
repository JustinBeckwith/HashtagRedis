namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Details necessary to generate a token for single-sign-on.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/windowsazure")]
    public class SsoToken : IExtensibleDataObject
    {
        /// <summary>
        /// Timestamp to indicate when the token was generated
        /// </summary>
        [DataMember(Order = 1)]
        public string TimeStamp { get; set; }

        /// <summary>
        /// The token to return to Windows Azure Runtime
        /// </summary>
        [DataMember(Order = 2)]
        public string Token { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}

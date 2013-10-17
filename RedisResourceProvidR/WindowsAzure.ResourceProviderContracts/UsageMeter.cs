namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Usage meter description.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class UsageMeter : IExtensibleDataObject
    {
        /// <summary>
        /// Included quantity.
        /// </summary>
        [DataMember(Order = 1)]
        public string Included { get; set; }

        /// <summary>
        /// Meter name.
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Meter unit.
        /// </summary>
        [DataMember(Order = 3)]
        public string Unit { get; set; }

        /// <summary>
        /// Used quantity.
        /// </summary>
        [DataMember(Order = 4)]
        public string Used { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

}

namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Property passed as part of Windows Azure subscription event.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementDataContractNamespace)]
    public class EntityProperty
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public string PropertyName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string PropertyValue { get; set; }
    }
}

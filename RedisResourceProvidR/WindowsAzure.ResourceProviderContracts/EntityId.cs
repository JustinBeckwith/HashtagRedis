namespace WindowsAzure.ResourceProviderContracts
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Identifier of an entity.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementDataContractNamespace)]
    public class EntityId
    {
        [DataMember(EmitDefaultValue = false, IsRequired = true, Order = 0)]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public DateTime Created { get; set; }
    }
}

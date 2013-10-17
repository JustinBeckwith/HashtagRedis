namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Subscription notification event.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementDataContractNamespace)]
    public class EntityEvent
    {
        [DataMember(EmitDefaultValue = false, IsRequired = true, Order = 0)]
        public string EventId { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string ListenerId { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string EntityType { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public EntityState EntityState { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)]
        public EntityId EntityId { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 5)]
        public string OperationId { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 6)]
        public bool IsAsync { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 7)]
        public EntityPropertyCollection Properties { get; set; }
    }
}

namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// State of an entity. Example of entities include Windows Azure Subscriptions.
    /// </summary>
    [DataContract(Namespace = Constants.ServiceManagementDataContractNamespace)]
    public enum EntityState
    {
        [EnumMember]
        Deleted,

        [EnumMember]
        Enabled,

        [EnumMember]
        Disabled,

        [EnumMember]
        Migrated,

        [EnumMember]
        Updated,

        [EnumMember]
        Registered,

        [EnumMember]
        Unregistered
    }
}

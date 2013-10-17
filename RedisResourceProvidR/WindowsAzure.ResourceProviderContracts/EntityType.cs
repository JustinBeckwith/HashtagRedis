namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// EntityType on which notifications are supported.
    /// </summary>
    [DataContract]
    public enum EntityType
    {
        [EnumMember]
        Subscription
    }
}

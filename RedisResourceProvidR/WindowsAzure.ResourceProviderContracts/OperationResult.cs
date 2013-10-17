namespace WindowsAzure.ResourceProviderContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Possible result from an operation.
    /// </summary>
    [DataContract(Namespace = Constants.WindowsAzureDataContractNamespace)]
    public enum OperationResult
    {
        [EnumMember]
        InProgress,

        [EnumMember]
        Succeeded,

        [EnumMember]
        Failed
    }
}

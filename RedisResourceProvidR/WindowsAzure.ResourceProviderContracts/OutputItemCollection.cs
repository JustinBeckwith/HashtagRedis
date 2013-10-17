namespace WindowsAzure.ResourceProviderContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Collection of key-value-pairs for resource output.
    /// </summary>
    [CollectionDataContract(Name = "OutputItems", ItemName = "OutputItem", Namespace = Constants.WindowsAzureDataContractNamespace)]
    public class OutputItemCollection : List<OutputItem>
    {
        public OutputItemCollection()
        {
        }

        public OutputItemCollection(IEnumerable<OutputItem> outputs) : base(outputs)
        {
        }
    }
}

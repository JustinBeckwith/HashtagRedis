namespace WindowsAzure.ResourceProviderContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A collection of entity properties of subscription notification events.
    /// </summary>
    [CollectionDataContractAttribute(Namespace = Constants.ServiceManagementDataContractNamespace, Name = "Properties", ItemName = "EntityProperty")]
    public class EntityPropertyCollection : List<EntityProperty>
    {
        public EntityPropertyCollection()
        {
        }

        public EntityPropertyCollection(IEnumerable<EntityProperty> entityProperties) : base(entityProperties)
        {
        }
    }
}

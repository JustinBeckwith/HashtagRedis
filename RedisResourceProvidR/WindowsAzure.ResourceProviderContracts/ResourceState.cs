namespace WindowsAzure.ResourceProviderContracts
{
    /// <summary>
    /// State of a resource.
    /// </summary>
    public enum ResourceState
    {
        /// <summary>
        /// The resource state is unkown because an error occurred
        /// </summary>
        Unknown,

        /// <summary>
        /// The resource provider has no record of this resource
        /// </summary>
        NotFound,

        /// <summary>
        /// The resource is started
        /// </summary>
        Started,

        /// <summary>
        /// The resource is stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// The resource is paused
        /// </summary>
        Paused
    }
}

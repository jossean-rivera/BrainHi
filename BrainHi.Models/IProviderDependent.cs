namespace BrainHi.Models
{
    /// <summary>
    /// Object that is a dependent of a <see cref="Provider"/>
    /// </summary>
    public interface IProviderDependent : IBrainHiObject
    {
        /// <summary>
        /// Identifier of the provider that the object depens on
        /// </summary>
        int ProviderId { get; set; }

        /// <summary>
        /// Navigation property to the provider that the object depens on
        /// </summary>
        Provider Provider { get; set; }
    }
}

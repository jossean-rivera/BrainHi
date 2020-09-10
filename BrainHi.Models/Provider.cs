using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BrainHi.Models
{
    /// <summary>
    /// Doctor or healthcare provider in the market
    /// </summary>
    [DisplayColumn(nameof(ProviderFullName))]
    public class Provider : IBrainHiObject
    {
        /// <summary>
        /// Primary identifier of the provider
        /// </summary>
        [Key]
        [JsonPropertyName("id")]
        [Display(Name = "ID")]
        public int ProviderId { get; set; }

        /// <summary>
        /// Full name of the provider
        /// </summary>
        [Required]
        [StringLength(255)]
        [JsonPropertyName("provider_full_name")]
        [Display(Name = "Full Name")]
        public string ProviderFullName { get; set; }

        /// <summary>
        /// Medical specialty of this Provider
        /// </summary>
        [Required]
        [StringLength(128)]
        [JsonPropertyName("specialty")]
        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        /// <summary>
        /// Navigation property of the appointments that the provider has
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        //  Implement interface explicitly. Map Id to the ProviderId
        int IBrainHiObject.Id { get => ProviderId; set => ProviderId = value; }
    }
}

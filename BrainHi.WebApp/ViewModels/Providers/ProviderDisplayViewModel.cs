using BrainHi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BrainHi.WebApp.ViewModels.Providers
{
    /// <summary>
    /// Required data to display view with details of a provider
    /// </summary>
    public class ProviderDisplayViewModel
    {
        /// <summary>
        /// Provider data to display
        /// </summary>
        public Provider Provider { get; set; }

        /// <summary>
        /// Unique identifier of the Provider associated with this Appointment
        /// </summary>
        [Required]
        [JsonPropertyName("provider_id")]
        [Display(Name = "Provider ID")]
        public int ProviderId { get; set; }

        /// <summary>
        /// Number of the month for the appointment
        /// </summary>
        [Required]
        [Range(1, 12)]
        public int? Month { get; set; }

        /// <summary>
        /// Number of the day for the appointment date
        /// </summary>
        [Required]
        [Range(1, 31)]
        public int? Day { get; set; }

        /// <summary>
        /// Number of year for the appointment date
        /// </summary>
        [Required]
        [Range(2020, 2099)]
        public int? Year { get; set; }

        /// <summary>
        /// Number of hour for the appointment time
        /// </summary>
        [Required]
        [Range(1, 24)]
        public int? Hour { get; set; }

        /// <summary>
        /// Number of minute for the appointment time
        /// </summary>
        [Required]
        [Range(0, 59)]
        public int? Minute { get; set; }

        /// <summary>
        /// Text describing why the patient booked and Appointment
        /// </summary>
        [Required]
        [Display(Name = "Reason")]
        [JsonPropertyName("appointment_reason")]
        [StringLength(4000)]
        public string AppointmentReason { get; set; }

        /// <summary>
        /// Full name of the Patient that requested the Appointment
        /// </summary>
        [Required]
        [Display(Name = "Patient Full Name")]
        [JsonPropertyName("patient_full_name")]
        [StringLength(128)]
        public string PatientFullName { get; set; }

        /// <summary>
        /// Gender of the Patient that requested the Appointment
        /// </summary>
        [Required]
        [Display(Name = "Patient Gender")]
        [JsonPropertyName("patient_gender")]
        public PatientGender? PatientGender { get; set; }

        /// <summary>
        /// Number of the month for the patient's date of birth
        /// </summary>
        [Required]
        [Range(1, 12)]
        [Display(Name = "Month")]
        public int? PatientDoBMonth { get; set; }

        /// <summary>
        /// Number of the day for the patient's date of birth
        /// </summary>
        [Required]
        [Range(1, 31)]
        [Display(Name = "Day")]
        public int? PatientDoBDay { get; set; }

        /// <summary>
        /// Number of year for the patient's date of birth
        /// </summary>
        [Required]
        [Range(1900, 2020)]
        [Display(Name = "Year")]
        public int? PatientDoBYear { get; set; }

        /// <summary>
        /// Phone number of the Patient that requested the Appointment
        /// </summary>
        [Required]
        [Phone]
        [StringLength(16)]
        [Display(Name = "Patient Phone Number")]
        [JsonPropertyName("patient_phone_number")]
        public string PatientPhoneNumber { get; set; }
    }
}

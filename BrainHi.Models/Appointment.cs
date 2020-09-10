using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BrainHi.Models
{
    /// <summary>
    /// Meeting set at a specific time for a patient to see a doctor
    /// </summary>
    public class Appointment : IBrainHiObject, IProviderDependent
    {
        /// <summary>
        /// Primary identifier of the appointment
        /// </summary>
        [Key]
        [JsonPropertyName("id")]
        [Display(Name = "ID")]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Unique identifier of the Provider associated with this Appointment
        /// </summary>
        [Required]
        [JsonPropertyName("provider_id")]
        [Display(Name = "Provider ID")]
        [ForeignKey(nameof(Provider))]
        public int ProviderId { get; set; }

        /// <summary>
        /// Navigation property of the provider associated with the appointment
        /// </summary>
        public Provider Provider { get; set; }

        /// <summary>
        /// Date and time the appointment starts
        /// </summary>
        [Required]
        [Display(Name = "Start Time")]
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Date and time the appointment ends
        /// </summary>
        [Required]
        [Display(Name = "End Time")]
        [JsonPropertyName("end_time")]
        public DateTime EndTime { get; set; }

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
        /// Date of birth of the Patient that requested the Appointment
        /// </summary>
        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "Patient Date of Birth", ShortName = "Patient DOB")]
        [JsonPropertyName("patient_date_of_birth")]
        public DateTime PatientDoB { get; set; }

        /// <summary>
        /// Phone number of the Patient that requested the Appointment
        /// </summary>
        [Required]
        [Phone]
        [StringLength(16)]
        [Display(Name = "Patient Phone Number")]
        [JsonPropertyName("patient_phone_number")]
        public string PatientPhoneNumber { get; set; }

        //  Implement interface explicitly. Map Id to the AppointmentId
        int IBrainHiObject.Id { get => AppointmentId; set => AppointmentId = value; }
    }
}

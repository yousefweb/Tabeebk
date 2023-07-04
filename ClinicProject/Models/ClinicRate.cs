using MimeKit.Encodings;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicProject.Models
{
    public class ClinicRate
    {
        public Guid ClinicRateId { get; set; }
        public int RateOfPAtient { get; set; }


        [ForeignKey("PatientId")]
        public Guid PatientId { get; set; } 
        public Patient Patient { get; set; }
        

        [ForeignKey("ClinicId")]
        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; }
    }
}

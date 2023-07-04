using ClinicProject.Data.Enum;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;


namespace ClinicProject.Models
{
    public class Clinic
    {
  
        public Guid ClinicId { get; set; }
        public string Clinic_Name { get; set; }
        public LocationState Clinic_Location { get; set; }
        public string Clinc_Description { get; set; }
        public int ClinicRate { get; set; }
        public double clinc_Price { get; set; }

        [ForeignKey("Clinic_DoctorId")]
        public Guid Clinic_DoctorId { get; set; }
        public Doctor Clinic_Doctor { get; set; }
        public List<ClinicDay> Clinic_Days { get; set; }
        public SituationOfReservation situationOfReservation { get; set; }
        public List<Reservation> Clinic_Reservations { get; set; }
        public string DoctorRequest_ReceivePatientNote { get; set; }
        public string Days { get; set; }

        public ICollection<ClinicRate> ClinicRates { get; set; }

    }
}

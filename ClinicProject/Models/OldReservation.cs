using ClinicProject.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace ClinicProject.Models
{
    public class OldReservation
    {
        public Guid OldReservationId { get; set; }
        [ForeignKey("Reservation_DoctorId")]
        public Guid OldReservation_DoctorId { get; set; }
        [ForeignKey("Reservation_PatientId")]
        public Guid OldReservation_PatientId { get; set; }
        public Doctor OldReservation_Doctor { get; set; }
        public Patient OldReservation_Patient { get; set; }
        public AppointmentType OldReservation_AppointmentType { get; set; }
        public DateTime Strat_Oldreservation { get; set; }
        public DateTime End_Oldreservation { get; set; }
        public Days OldReservation_Day { get; set; }
        public Specialization OldReservation_Specialization { get; set; }
        // public string Amount { get; set; }
        public SituationOfReservation situationOfOldReservation { get; set; }

        public Guid TimeSlotId_Oldreservation { get; set; }

    }
}

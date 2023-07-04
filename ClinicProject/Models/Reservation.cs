using ClinicProject.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Reservation
    {

        public Guid ReservationId { get; set; }
        [ForeignKey("Reservation_DoctorId")]
        public Guid Reservation_DoctorId { get; set; }
        [ForeignKey("Reservation_PatientId")]
        public Guid Reservation_PatientId { get; set; }
        public Doctor Reservation_Doctor { get; set; }
        public Patient Reservation_Patient { get; set; }
        public AppointmentType Reservation_AppointmentType { get; set; }
        public DateTime Strat_reservation { get; set; }
        public DateTime End_reservation { get; set; }
        public Days Reservation_Day { get; set; }
        public Specialization Reservation_Specialization { get; set;}

        public double ReservationAmount { get; set; }
        public SituationOfReservation situationOfReservation { get; set; }

        public Guid TimeSlotId_reservation { get; set; }

    }
}

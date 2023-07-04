using System;

namespace ClinicProject.Models
{
    public class PatientNoteToDoctor
    {
        public Guid Id { get; set; }
        public string Note{ get; set; }
        public Guid PatientId { get; set; }
        public Patient patient { get; set; }
        public Guid DoctorId { get; set; }    
        public Doctor doctor { get; set; }
        public DateTime MessageTime { get; set; }

    }
}

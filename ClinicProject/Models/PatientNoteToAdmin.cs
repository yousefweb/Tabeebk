using System;

namespace ClinicProject.Models
{
    public class PatientNoteToAdmin
    {
        public Guid id { get; set; }
        public string Discription { get; set; }
        public Guid PatientId { get; set; }
        public Patient patient { get; set; }
        public DateTime MessageTime { get; set; }


    }
}
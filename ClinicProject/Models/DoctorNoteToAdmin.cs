using System;

namespace ClinicProject.Models
{
    public class DoctorNoteToAdmin
    {
        public Guid Id { get; set; }
        public string Discription { get; set; }
        public Guid DoctorId { get; set; }
        public Doctor doctor { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
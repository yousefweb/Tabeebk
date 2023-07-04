using ClinicProject.Data.Enum;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;

namespace ClinicProject.Models
{
    public class ClinicDay
    {

        public Guid ClinicDayId { get; set; }
        public Days DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid ClinicId { get; set; }
        public Clinic Clinic { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; }
        public DateTime DateOfWork { get; set; }

    }
}

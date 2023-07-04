using System;

namespace ClinicProject.Models
{
    public class TimeSlot
    {

        public Guid TimeSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ClinicDayId { get; set; }
        public ClinicDay ClinicDay { get; set; }
    }
}

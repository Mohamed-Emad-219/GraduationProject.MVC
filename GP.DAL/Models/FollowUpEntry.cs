using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class FollowUpEntry
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; } // FK to InstructorSchedule or AssistantSchedule
        public InstructorSchedule? Schedule { get; set; }

        public int WeekNumber { get; set; } // 1–14
        public bool? Attended { get; set; }

        public DateTime Date { get; set; }

        public int FollowUpId { get; set; } // Foreign key
        public FollowUp? FollowUp { get; set; } // Navigation property
    }
}

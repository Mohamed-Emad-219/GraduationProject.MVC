﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class FollowUpSchedule 
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public string Day { get; set; }
        [Column(TypeName = "time")] // SQL Server's time type
        public TimeSpan TimeBegin { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan TimeEnd { get; set; }
        public SemesterType Semester { get; set; }
        public int AcademicYear { get; set; }
        public int PlaceId { get; set; }
        public int? InstructorId { get; set; }
        public int? AssistantId { get; set; }
        public string CourseCode { get; set; }
        public Place Place { get; set; }
        public Course Course { get; set; }
        public FacultyMember? Instructor { get; set; }
        public FacultyMember? Assistant { get; set; }
        public int Week { get; set; }
        public bool IsAttendant { get; set; }
        public int FollowUpId { get; set; }
        public FollowUp FollowUp { get; set; }
    }
}

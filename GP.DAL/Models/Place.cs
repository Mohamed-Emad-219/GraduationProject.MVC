using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Capacity { get; set; }
        [RegularExpression(@"^(Lab|L|Ex|L/Ex|Ex/Lab)$", ErrorMessage = "Type must be Lab, L, Ex, L/Ex, or Ex/Lab.")]
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<StudentSchedule> StudentSchedules { get; set; }
        public ICollection<InstructorSchedule> InstructorSchedules { get; set; }
    }
}

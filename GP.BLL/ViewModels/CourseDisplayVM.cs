using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.BLL.ViewModels
{
    public class CourseDisplayVM
    {
        public string FacultyName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int? Credits { get; set; }
        public int? WeeklyHours { get; set; }
    }
}

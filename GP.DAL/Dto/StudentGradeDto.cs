using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Dto
{
    public class StudentGradeDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public double GPA { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class CoursesTerm
    {
        public int TermId { get; set; }
        public Term Term { get; set; }
        public string CourseCode { get; set; }
        public Course Course { get; set; }
        public int Price { get; set; }
    }
}

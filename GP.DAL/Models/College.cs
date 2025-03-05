using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class College
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // One-to-One relationship for the Head of the Department
        public int? DeanId { get; set; }
        public FacultyMember? Dean { get; set; }

        // One-to-Many relationship for Department
        public ICollection<Department>? Departments { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Receipt>? Receipts { get; set; }

    }
}

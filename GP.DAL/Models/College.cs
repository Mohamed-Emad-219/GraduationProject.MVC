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
        public string? DeanId { get; set; }
        public FacultyMember? Dean { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Receipt>? Receipts { get; set; }

    }
}

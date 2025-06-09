using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Application
    {
        public int  Id  { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Status Status { get; set; }
        public int? StudentAffairsId  { get; set; }
        public StudentAffairs? StudentAffairs { get; set; }
        public string StudentId { get; set; }
        public Student? Student { get; set; }

    }
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }
}

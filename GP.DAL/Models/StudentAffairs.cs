﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class StudentAffairs
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public GPUser User { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "SSN must be exactly 16 digits.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "SSN must contain only numbers.")]
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Currency)]
        public double Salary { get; set; }
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Mobile phone must be exactly 11 digits.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Mobile phone must contain only numbers.")]
        public string MobilePhone { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public int? ManagerId { get; set; }
        public StudentAffairs? Manager { get; set; }
        public ICollection<StudentAffairs> Subordinates { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
        public ICollection<Application> Applications { get; set; }

    }
}

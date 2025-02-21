﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.DAL.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public string Semester { get; set; }
        public int AcademicYear { get; set; }
        [DataType(DataType.Currency)]
        public double AmountDue {  get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public int CollegeId { get; set; }
        public int FinancialAffairsId { get; set; }
        public int StudentAffairsId { get; set; }
        public College College { get; set; }
        public FinancialAffairs FinancialAffairs { get; set; }
        public StudentAffairs StudentAffairs { get; set; }
    }
}

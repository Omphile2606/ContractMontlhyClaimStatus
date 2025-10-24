using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractMontlhyClaimStatus.Repositories;

namespace ContractMonthlyClaimSystem.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string Notes { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentFullPath { get; set; }
        public string Status { get; set; } = "Pending";

        public decimal TotalAmount => HoursWorked * HourlyRate;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ContractMontlhyClaimStatus.Models
{
    using System;
        public class Claim
        {
            public int Id { get; set; }
            public string LecturerName { get; set; }
            public int HoursWorked { get; set; }
            public decimal HourlyRate { get; set; }
            public string Notes { get; set; }
            public string DocumentFileName { get; set; }    // filename only
            public string DocumentFullPath { get; set; }    // full path if needed
            public string Status { get; set; } = "Pending";

            public decimal TotalAmount => HoursWorked * HourlyRate;
        }
    }
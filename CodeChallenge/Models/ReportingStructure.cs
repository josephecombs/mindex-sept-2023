using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }

        public ReportingStructure(Employee employee, int numberOfReports)
        {
            this.Employee = employee;
            this.NumberOfReports = numberOfReports;
        }
    }
}

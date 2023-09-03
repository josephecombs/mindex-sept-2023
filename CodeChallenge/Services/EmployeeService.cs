using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public List<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        public ReportingStructure GetReportingStructure(string employeeId)
        {
            // Fetch the root employee
            Employee rootEmployee = _employeeRepository.GetById(employeeId);

            // If employee doesn't exist, return null
            if (rootEmployee == null) return null;

            // Compute the number of reports recursively
            int numberOfReports = CalculateNumberOfReports(rootEmployee);

            // Return the reporting structure
            return new ReportingStructure(rootEmployee, numberOfReports);
        }

        // Recursive function to calculate the number of direct reports
        private int CalculateNumberOfReports(Employee employee)
        {
            // Initialize count with zero for the base case (an employee with no direct reports)
            int count = 0;

            // If the employee has no direct reports, this will be null or an empty list, and the loop will be skipped
            if (employee.DirectReports != null)
            {
                // Count each direct report and their respective direct reports
                foreach (Employee directReport in employee.DirectReports)
                {
                    // Increment by one for the direct report itself
                    count++;

                    // Recursively count the direct reports of the direct report
                    count += CalculateNumberOfReports(directReport);
                }
            }

            return count;
        }
    }
}

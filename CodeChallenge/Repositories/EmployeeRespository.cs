using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            var allEmployees = _employeeContext.Employees.Include(e => e.DirectReports).Include(e => e.Compensation).ToList();
            return allEmployees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public List<Employee> GetAll()
        {
            return _employeeContext.Employees.ToList();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        // stuff for Compensation
        public void AddCompensation(string employeeId, Compensation compensation)
        {
            // Here you'd look up the Employee to which this Compensation should be attached.
            var employee = _employeeContext.Employees.Find(employeeId);
            if (employee != null)
            {
                // If the Employee is found, associate it with the Compensation.
                compensation.EmployeeId = employeeId;
                
                // Add the compensation to the Employee
                employee.Compensation = compensation;
                
                // Or if you're tracking compensations separately, add to the DbSet directly.
                _employeeContext.Compensation.Add(compensation);
            }
            else
            {
                // Handle the situation where the Employee was not found,
                // perhaps throw an exception or return a boolean for success/failure.
            }
        }

        public Compensation GetCompensationById(string id)
        {
            return _employeeContext.Compensation
                .SingleOrDefault(c => c.EmployeeId == id);
        }
    }
}

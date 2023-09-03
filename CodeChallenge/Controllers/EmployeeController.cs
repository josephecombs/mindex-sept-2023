using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("{id}/reporting-structure", Name = "getReportingStructure")]        
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received Reporting Structure get request for '{id}'");

            // Call the GetReportingStructure method from the service layer
            ReportingStructure reportingStructure = _employeeService.GetReportingStructure(id);

            // If the employee or reporting structure does not exist, return a 404 Not Found status
            if (reportingStructure == null)
            {
                return NotFound();
            }

            // If everything is successful, return a 200 OK status along with the reporting structure
            return Ok(reportingStructure);
        }


        [HttpGet("all", Name = "getAllEmployees")]  // Explicitly set route to /api/employee/all
        [Route("/api/employees")]  // Override for this specific action to /api/employees
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAll();  // Assuming you implement GetAll() in your service
            return Ok(employees);
        }
    }
}

﻿using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Employee GetById(String id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
        
        ReportingStructure GetReportingStructure(String id);

        Compensation CreateCompensation(string id, Compensation compensation);
        Compensation GetCompensationById(string id);
    }
}

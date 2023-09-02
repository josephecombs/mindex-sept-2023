using CodeChallenge.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        List<Employee> GetAll();
        Task SaveAsync();
    }
}
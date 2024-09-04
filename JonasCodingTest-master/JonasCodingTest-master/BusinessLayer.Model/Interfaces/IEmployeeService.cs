using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Model.Interfaces
{
    public interface IEmployeeService
    {
        // Get all employees
        IEnumerable<Employee> GetAllEmployees();

        // Get a specific employee by code
        Employee GetEmployeeByCode(string employeeCode);

        // Add a new employee
        void AddEmployee(Employee employee);

        // Update an existing employee
        void UpdateEmployee(Employee employee);

        // Delete an employee by code
        void DeleteEmployee(String employeeCode);
    }
}

using BusinessLayer.Model.Interfaces;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{

   
    public class EmployeeService : IEmployeeService
    {

        private ILogger<EmployeeService> _logger;

        private readonly IEmployeeRepository _employeeRepository;

        // Constructor to inject the employee repository dependency
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;

        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            try
            {
                return _employeeRepository.GetAllEmployees();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all employees.");
                throw new Exception("Error retrieving employee data.");
            }
        }

        public Employee GetEmployeeByCode(string employeeCode)
        {
            try
            {
                var employee = _employeeRepository.GetEmployeeByCode(employeeCode);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with code {EmployeeCode} not found.", employeeCode);
                    throw new KeyNotFoundException("Employee not found.");
                }
                return employee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving employee with code {EmployeeCode}.", employeeCode);
                throw new Exception("Error retrieving employee data.");
            }
        }

        public void AddEmployee(Employee employee)
        {
            try
            {
                _employeeRepository.Add(employee);
                _logger.LogInformation("Employee with code {EmployeeCode} added successfully.", employee.EmployeeCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding employee with code {EmployeeCode}.", employee.EmployeeCode);
                throw new Exception("Error adding employee.");
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            try
            {
                _employeeRepository.Update(employee);
                _logger.LogInformation("Employee with code {EmployeeCode} updated successfully.", employee.EmployeeCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating employee with code {EmployeeCode}.", employee.EmployeeCode);
                throw new Exception("Error updating employee.");
            }
        }

        public void DeleteEmployee(string employeeCode)
        {
            try
            {
                Employee employee = _employeeRepository.GetEmployeeByCode(employeeCode);
                if (employee == null)
                {
                    _logger.LogWarning("Employee with code {EmployeeCode} not found.", employeeCode);
                    throw new KeyNotFoundException("Employee not found.");
                }

                _employeeRepository.Delete(employee);
                _logger.LogInformation("Employee with code {EmployeeCode} deleted successfully.", employeeCode);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Attempted to delete non-existent employee with code {EmployeeCode}.", employeeCode);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee with code {EmployeeCode}.", employeeCode);
                throw new Exception("Error deleting employee.");
            }
        }
    }
}

using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper, ILogger<EmployeeRepository> logger)
        {
            _employeeDbWrapper = employeeDbWrapper;
            _logger = logger;
        }

        // Retrieves all employees from the database.
        public IEnumerable<Employee> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Retrieving all employees from the database.");
                return _employeeDbWrapper.FindAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all employees.");
                throw;
            }
        }

        // Retrieves a specific employee by their employee code.
        public Employee GetEmployeeByCode(string employeeCode)
        {
            try
            {
                _logger.LogInformation("Retrieving employee with EmployeeCode: {EmployeeCode}", employeeCode);
                return _employeeDbWrapper.Find(e => e.EmployeeCode.Equals(employeeCode))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the employee with code {EmployeeCode}", employeeCode);
                throw;
            }
        }

        // Saves an employee to the database. Updates if the employee already exists, otherwise inserts a new one.
        public bool SaveEmployee(Employee employee)
        {
            try
            {
                _logger.LogInformation("Saving employee with EmployeeCode: {EmployeeCode}", employee.EmployeeCode);
                var existingEmployee = _employeeDbWrapper.Find(e =>
                    e.SiteId.Equals(employee.SiteId) && e.EmployeeCode.Equals(employee.EmployeeCode))?.FirstOrDefault();

                if (existingEmployee != null)
                {
                    _logger.LogInformation("Updating existing employee with EmployeeCode: {EmployeeCode}", employee.EmployeeCode);
                    existingEmployee.EmployeeName = employee.EmployeeName;
                    existingEmployee.Occupation = employee.Occupation;
                    existingEmployee.EmployeeStatus = employee.EmployeeStatus;
                    existingEmployee.EmailAddress = employee.EmailAddress;
                    existingEmployee.Phone = employee.Phone;
                    existingEmployee.LastModified = employee.LastModified;
                    return _employeeDbWrapper.Update(existingEmployee);
                }

                _logger.LogInformation("Inserting new employee with EmployeeCode: {EmployeeCode}", employee.EmployeeCode);
                return _employeeDbWrapper.Insert(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the employee with code {EmployeeCode}", employee.EmployeeCode);
                throw;
            }
        }

        public void Add(Employee employee)
        {
            if (employee == null)
            {
                _logger.LogWarning("Attempted to add a null employee.");
                throw new ArgumentNullException(nameof(employee));
            }

            try
            {
                _logger.LogInformation("Adding new employee with EmployeeCode: {EmployeeCode}", employee.EmployeeCode);
                _employeeDbWrapper.Insert(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the employee with code {EmployeeCode}", employee.EmployeeCode);
                throw;
            }
        }

        // Deletes an employee from the database.
        public void Delete(Employee employee)
        {
            if (employee == null)
            {
                _logger.LogWarning("Attempted to delete a null employee.");
                throw new ArgumentNullException(nameof(employee));
            }

            try
            {
                _logger.LogInformation("Deleting employee with EmployeeCode: {EmployeeCode}", employee.EmployeeCode);
                _employeeDbWrapper.Delete(e =>
                    e.SiteId.Equals(employee.SiteId) && e.EmployeeCode.Equals(employee.EmployeeCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employee with code {EmployeeCode}", employee.EmployeeCode);
                throw;
            }
        }

        // Updates an existing employee in the database.
        public void Update(Employee employee)
        {
            if (employee == null)
            {
                _logger.LogWarning("Attempted to update a null employee.");
                throw new ArgumentNullException(nameof(employee));
            }

            try
            {
                _logger.LogInformation("Updating employee with EmployeeCode: {EmployeeCode}", employee.EmployeeCode);
                _employeeDbWrapper.Update(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the employee with code {EmployeeCode}", employee.EmployeeCode);
                throw;
            }
        }
    }
}

using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
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

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }


        // Retrieves all employees from the database.
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeDbWrapper.FindAll();
        }

        // Retrieves a specific employee by their employee code.
        public Employee GetEmployeeByCode(string employeeCode)
        {
            return _employeeDbWrapper.Find(e => e.EmployeeCode.Equals(employeeCode))?.FirstOrDefault();
        }


        // Saves an employee to the database. Updates if the employee already exists, otherwise inserts a new one.
        public bool SaveEmployee(Employee employee)
        {
            var existingEmployee = _employeeDbWrapper.Find(e =>
                e.SiteId.Equals(employee.SiteId) && e.EmployeeCode.Equals(employee.EmployeeCode))?.FirstOrDefault();

            if (existingEmployee != null)
            {
                existingEmployee.EmployeeName = employee.EmployeeName;
                existingEmployee.Occupation = employee.Occupation;
                existingEmployee.EmployeeStatus = employee.EmployeeStatus;
                existingEmployee.EmailAddress = employee.EmailAddress;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.LastModified = employee.LastModified;
                return _employeeDbWrapper.Update(existingEmployee);
            }

            return _employeeDbWrapper.Insert(employee);
        }

        public void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _employeeDbWrapper.Insert(employee);
        }


        // Deletes an employee from the database.
        public void Delete(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _employeeDbWrapper.Delete(e =>
                e.SiteId.Equals(employee.SiteId) && e.EmployeeCode.Equals(employee.EmployeeCode));
        }



        // Updates an existing employee in the database.
        public void Update(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _employeeDbWrapper.Update(employee);
        }
    }
}

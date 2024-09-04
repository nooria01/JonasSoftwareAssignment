using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model.Interfaces
{
    public interface IEmployeeRepository
    {

        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeByCode(string employeeCode);
        bool SaveEmployee(Employee employee);
        void Add(Employee employee);
        void Delete(Employee employee);
        void Update(Employee employee);
    }
}

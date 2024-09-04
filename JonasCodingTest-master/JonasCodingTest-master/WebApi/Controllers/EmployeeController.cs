using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using AutoMapper;

namespace WebApi.Controllers
{

    // Controller for managing employee-related operations
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        // GET api/employee/{employeeCode}
        [HttpGet("{employeeCode}")]
        public IActionResult GetEmployeeByCode(string employeeCode)
        {
            var employee = _employeeService.GetEmployeeByCode(employeeCode);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<Employee>(employee);
            return Ok(employeeDto);
        }

        // GET api/employee
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            var employeeDtos = _mapper.Map<IEnumerable<Employee>>(employees);
            return Ok(employeeDtos);
        }

        // POST api/employee
        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest();
            }

            var employee = _mapper.Map<Employee>(employeeDto);
            _employeeService.AddEmployee(employee);

            return CreatedAtAction(nameof(GetEmployeeByCode), new { employeeCode = employeeDto.EmployeeCode }, employeeDto);
        }

        // PUT api/employee/{employeeCode}
        [HttpPut("{employeeCode}")]
        public IActionResult UpdateEmployee(string employeeCode, [FromBody] Employee employeeDto)
        {
            if (employeeDto == null || employeeCode != employeeDto.EmployeeCode)
            {
                return BadRequest();
            }

            var employee = _mapper.Map<Employee>(employeeDto);
            _employeeService.UpdateEmployee(employee);

            return NoContent();
        }

        // DELETE api/employee/{employeeCode}
        [HttpDelete("{employeeCode}")]
        public IActionResult DeleteEmployee(string employeeCode)
        {
            _employeeService.DeleteEmployee(employeeCode);
            return NoContent();
        }

    }
}
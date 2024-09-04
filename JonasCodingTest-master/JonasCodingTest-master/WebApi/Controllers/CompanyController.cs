using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{

    // Controller for managing company-related operations
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        

        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        // Constructor to inject the ICompanyService and IMapper dependencies
        public CompanyController(ICompanyService companyService , IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }


        // GET api/<controller>
        // Fetches all companies asynchronously
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return (IActionResult)Ok(companies);
        }


        // GET api/<controller>/companyCode
        // Fetches a single company by its code asynchronously
        [HttpGet("{companyCode}")]
        public async Task<IActionResult> GetCompanyByCode(string companyCode)
        {
            var company = await _companyService.GetCompanyByCodeAsync(companyCode);

            if (company == null)
            {
                return NotFound();
            }
            return (IActionResult)Ok(company);
        }


        // POST api/<controller>
        // Adds a new company asynchronously
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto company)

        {

            // Map the DTO to the entity model
            var entity =   _mapper.Map<CompanyInfo>(company);


            // Add the company through the service
            await _companyService.AddCompanyAsync(entity);


            // Return CreatedAtAction to indicate that the resource was created successfully
            return CreatedAtAction(nameof(GetCompanyByCode), new { companyCode = company.CompanyCode }, company);
        }


        // PUT api/<controller>/companyCode
        // Updates an existing company asynchronously
       [HttpPut("{companyCode}")]
        public async Task<IActionResult> UpdateCompany(string companyCode, CompanyDto company)
        {

            // Check if the provided company code matches the one in the DTO

            if (companyCode != company.CompanyCode)
            {
                return BadRequest();
            }
            var entity = _mapper.Map<CompanyInfo>(company);
            await _companyService.UpdateCompanyAsync(entity);
            return NoContent();        // Return NoContent to indicate that the update was successful but no content is returned

        }

        [HttpDelete("{companyCode}")]// Deletes a company by its code asynchronously
        public async Task<IActionResult> DeleteCompany(string companyCode)
        {
            await _companyService.DeleteCompanyAsync(companyCode);
            return NoContent();
        }

    }
}
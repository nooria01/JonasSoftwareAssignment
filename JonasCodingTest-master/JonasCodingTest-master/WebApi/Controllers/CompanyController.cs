using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{

    // Controller for managing company-related operations
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyController> _logger;

        // Constructor to inject the ICompanyService, IMapper, and ILogger dependencies
        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>
        // Fetches all companies asynchronously
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                _logger.LogInformation("Fetching all companies.");
                var companies = await _companyService.GetAllCompaniesAsync();
                return Ok(companies); // Return 200 OK with the list of companies
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all companies.");
                return StatusCode(500, "Internal server error."); // Return 500 if there's an issue
            }
        }

        // GET api/<controller>/companyCode
        // Fetches a single company by its code asynchronously
        [HttpGet("{companyCode}")]
        public async Task<IActionResult> GetCompanyByCode(string companyCode)
        {
            try
            {
                _logger.LogInformation("Fetching company with code: {CompanyCode}", companyCode);
                var company = await _companyService.GetCompanyByCodeAsync(companyCode);

                if (company == null)
                {
                    _logger.LogWarning("Company with code {CompanyCode} not found.", companyCode);
                    return NotFound(); // Return 404 if the company is not found
                }

                return Ok(company); // Return 200 OK with the company details
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching company with code: {CompanyCode}", companyCode);
                return StatusCode(500, "Internal server error."); // Return 500 for errors
            }
        }

        // POST api/<controller>
        // Adds a new company asynchronously
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto company)
        {
            try
            {
                _logger.LogInformation("Adding new company with code: {CompanyCode}", company.CompanyCode);

                // Map the DTO to the entity model
                var entity = _mapper.Map<CompanyInfo>(company);

                // Add the company through the service
                await _companyService.AddCompanyAsync(entity);

                // Return 201 Created and location of the new company
                return CreatedAtAction(nameof(GetCompanyByCode), new { companyCode = company.CompanyCode }, company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding company with code: {CompanyCode}", company.CompanyCode);
                return StatusCode(500, "Internal server error."); // Return 500 for any failure
            }
        }

        // PUT api/<controller>/companyCode
        // Updates an existing company asynchronously
        [HttpPut("{companyCode}")]
        public async Task<IActionResult> UpdateCompany(string companyCode, CompanyDto company)
        {
            try
            {
                _logger.LogInformation("Updating company with code: {CompanyCode}", companyCode);

                // Check if the provided company code matches the one in the DTO
                if (companyCode != company.CompanyCode)
                {
                    _logger.LogWarning("Company code in URL does not match the code in the body.");
                    return BadRequest("Company code mismatch."); // Return 400 if the codes don't match
                }

                var entity = _mapper.Map<CompanyInfo>(company);
                await _companyService.UpdateCompanyAsync(entity);

                return NoContent(); // Return 204 No Content to indicate success with no response body
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating company with code: {CompanyCode}", companyCode);
                return StatusCode(500, "Internal server error."); // Return 500 for errors
            }
        }

        // DELETE api/<controller>/companyCode
        // Deletes a company by its code asynchronously
        [HttpDelete("{companyCode}")]
        public async Task<IActionResult> DeleteCompany(string companyCode)
        {
            try
            {
                _logger.LogInformation("Deleting company with code: {CompanyCode}", companyCode);
                await _companyService.DeleteCompanyAsync(companyCode);
                return NoContent(); // Return 204 No Content to indicate successful deletion
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting company with code: {CompanyCode}", companyCode);
                return StatusCode(500, "Internal server error."); // Return 500 for errors
            }
        }

    }
}
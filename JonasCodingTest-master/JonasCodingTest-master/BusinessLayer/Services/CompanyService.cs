using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        // Constructor to inject the repository, mapper, and logger
        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Get all companies from the database
        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all companies from the database.");
                var result = await _companyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyInfo>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all companies.");
                throw; // Rethrow the exception so the caller can handle it if necessary
            }
        }

        // Get a specific company by its code
        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            try
            {
                _logger.LogInformation("Retrieving company with code: {CompanyCode}", companyCode);
                var result = await _companyRepository.GetByCodeAsync(companyCode);
                if (result == null)
                {
                    _logger.LogWarning("Company with code {CompanyCode} not found.", companyCode);
                    throw new KeyNotFoundException("Company not found.");
                }
                return _mapper.Map<CompanyInfo>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving company with code {CompanyCode}.", companyCode);
                throw;
            }
        }

        // Add a new company to the database
        public async Task AddCompanyAsync(CompanyInfo company)
        {
            if (company == null)
            {
                _logger.LogWarning("Attempted to add a null company.");
                throw new ArgumentNullException(nameof(company));
            }

            try
            {
                _logger.LogInformation("Adding new company with name: {CompanyName}", company.CompanyName);
                var entity = _mapper.Map<Company>(company); // Map to the entity model
                await _companyRepository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new company.");
                throw;
            }
        }

        // Delete a company by its code
        public async Task DeleteCompanyAsync(string companyCode)
        {
            try
            {
                _logger.LogInformation("Deleting company with code: {CompanyCode}", companyCode);
                var entity = await _companyRepository.GetByCodeAsync(companyCode);
                if (entity == null)
                {
                    _logger.LogWarning("Company with code {CompanyCode} not found for deletion.", companyCode);
                    throw new KeyNotFoundException("Company not found.");
                }

                await _companyRepository.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the company with code {CompanyCode}.", companyCode);
                throw;
            }
        }

        // Update an existing company in the database
        public async Task UpdateCompanyAsync(CompanyInfo company)
        {
            if (company == null)
            {
                _logger.LogWarning("Attempted to update a null company.");
                throw new ArgumentNullException(nameof(company));
            }

            try
            {
                _logger.LogInformation("Updating company with code: {CompanyCode}", company.CompanyCode);
                var entity = _mapper.Map<Company>(company); // Map to the entity model
                await _companyRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the company with code {CompanyCode}.", company.CompanyCode);
                throw;
            }
        }
    }
}

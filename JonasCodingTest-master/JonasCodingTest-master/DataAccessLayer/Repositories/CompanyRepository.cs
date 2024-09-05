using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;
        private readonly ILogger<CompanyRepository> _logger;

        // Constructor to inject the database wrapper and logger
        public CompanyRepository(IDbWrapper<Company> companyDbWrapper, ILogger<CompanyRepository> logger)
        {
            _companyDbWrapper = companyDbWrapper;
            _logger = logger;
        }

        // Get all companies from the database
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all companies from the database.");
                return await _companyDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all companies.");
                throw; // Rethrow the exception so it can be handled by the caller
            }
        }

        // Get a company by its code
        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            try
            {
                _logger.LogInformation("Fetching company with code: {CompanyCode}", companyCode);
                return (await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching company with code: {CompanyCode}", companyCode);
                throw;
            }
        }

        // Save or update a company in the database
        public async Task<bool> SaveCompanyAsync(Company company)
        {
            if (company == null)
            {
                _logger.LogWarning("Attempted to save a null company.");
                throw new ArgumentNullException(nameof(company));
            }

            try
            {
                _logger.LogInformation("Saving company with code: {CompanyCode}", company.CompanyCode);

                var existingCompany = (await _companyDbWrapper.FindAsync(t =>
                    t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode)))?.FirstOrDefault();

                if (existingCompany != null)
                {
                    _logger.LogInformation("Company with code {CompanyCode} found. Updating it.", company.CompanyCode);

                    // Update existing company fields
                    existingCompany.CompanyName = company.CompanyName;
                    existingCompany.AddressLine1 = company.AddressLine1;
                    existingCompany.AddressLine2 = company.AddressLine2;
                    existingCompany.AddressLine3 = company.AddressLine3;
                    existingCompany.Country = company.Country;
                    existingCompany.EquipmentCompanyCode = company.EquipmentCompanyCode;
                    existingCompany.FaxNumber = company.FaxNumber;
                    existingCompany.PhoneNumber = company.PhoneNumber;
                    existingCompany.PostalZipCode = company.PostalZipCode;
                    existingCompany.LastModified = company.LastModified;

                    return await _companyDbWrapper.UpdateAsync(existingCompany);
                }

                // Insert new company
                _logger.LogInformation("Company with code {CompanyCode} not found. Inserting new company.", company.CompanyCode);
                return await _companyDbWrapper.InsertAsync(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving company with code: {CompanyCode}", company.CompanyCode);
                throw;
            }
        }

        // Add a new company to the database
        public async Task AddAsync(Company entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to add a null company.");
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _logger.LogInformation("Adding a new company with code: {CompanyCode}", entity.CompanyCode);
                await _companyDbWrapper.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding company with code: {CompanyCode}", entity.CompanyCode);
                throw;
            }
        }

        // Delete a company from the database
        public async Task DeleteAsync(Company entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to delete a null company.");
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _logger.LogInformation("Deleting company with code: {CompanyCode}", entity.CompanyCode);
                await _companyDbWrapper.DeleteAsync(t =>
                    t.SiteId.Equals(entity.SiteId) && t.CompanyCode.Equals(entity.CompanyCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting company with code: {CompanyCode}", entity.CompanyCode);
                throw;
            }
        }

        // Update an existing company in the database
        public async Task UpdateAsync(Company entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Attempted to update a null company.");
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _logger.LogInformation("Updating company with code: {CompanyCode}", entity.CompanyCode);

                var existingEntity = (await _companyDbWrapper.FindAsync(t =>
                    t.SiteId.Equals(entity.SiteId) && t.CompanyCode.Equals(entity.CompanyCode)))?.FirstOrDefault();

                if (existingEntity == null)
                {
                    _logger.LogWarning("Company with code {CompanyCode} not found for update.", entity.CompanyCode);
                    throw new KeyNotFoundException("Company not found.");
                }

                // Update the existing company fields
                existingEntity.CompanyName = entity.CompanyName;
                existingEntity.AddressLine1 = entity.AddressLine1;
                existingEntity.AddressLine2 = entity.AddressLine2;
                existingEntity.AddressLine3 = entity.AddressLine3;
                existingEntity.Country = entity.Country;
                existingEntity.EquipmentCompanyCode = entity.EquipmentCompanyCode;
                existingEntity.FaxNumber = entity.FaxNumber;
                existingEntity.PhoneNumber = entity.PhoneNumber;
                existingEntity.PostalZipCode = entity.PostalZipCode;
                existingEntity.LastModified = entity.LastModified;

                await _companyDbWrapper.UpdateAsync(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating company with code: {CompanyCode}", entity.CompanyCode);
                throw;
            }
        }
    }
}

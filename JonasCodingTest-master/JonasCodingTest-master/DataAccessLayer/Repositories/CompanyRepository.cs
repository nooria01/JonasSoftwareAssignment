using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;

        public CompanyRepository(IDbWrapper<Company> companyDbWrapper)
        {
            _companyDbWrapper = companyDbWrapper;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyDbWrapper.FindAllAsync();
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            return (await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode))).FirstOrDefault();
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            var itemRepo = (await _companyDbWrapper.FindAsync(t =>
                t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode)))?.FirstOrDefault();

            if (itemRepo != null)
            {
                itemRepo.CompanyName = company.CompanyName;
                itemRepo.AddressLine1 = company.AddressLine1;
                itemRepo.AddressLine2 = company.AddressLine2;
                itemRepo.AddressLine3 = company.AddressLine3;
                itemRepo.Country = company.Country;
                itemRepo.EquipmentCompanyCode = company.EquipmentCompanyCode;
                itemRepo.FaxNumber = company.FaxNumber;
                itemRepo.PhoneNumber = company.PhoneNumber;
                itemRepo.PostalZipCode = company.PostalZipCode;
                itemRepo.LastModified = company.LastModified;
                return await _companyDbWrapper.UpdateAsync(itemRepo);
            }

            return await _companyDbWrapper.InsertAsync(company);
        }

        public async Task AddAsync(Company entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _companyDbWrapper.InsertAsync(entity);
        }

        public async Task DeleteAsync(Company entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _companyDbWrapper.DeleteAsync(t =>
                t.SiteId.Equals(entity.SiteId) && t.CompanyCode.Equals(entity.CompanyCode));
        }

        public async Task UpdateAsync(Company entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingEntity = (await _companyDbWrapper.FindAsync(t =>
                t.SiteId.Equals(entity.SiteId) && t.CompanyCode.Equals(entity.CompanyCode)))?.FirstOrDefault();

            if (existingEntity == null)
                throw new KeyNotFoundException("Company not found");

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
    }
}

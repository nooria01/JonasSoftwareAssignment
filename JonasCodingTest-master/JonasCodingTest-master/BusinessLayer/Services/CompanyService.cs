using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        


        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            var res = await _companyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyInfo>>(res);
        }

        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            var result = await _companyRepository.GetByCodeAsync(companyCode);
            return _mapper.Map<CompanyInfo>(result);
        }

        public async Task AddCompanyAsync(CompanyInfo company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            var entity = _mapper.Map<Company>(company); // Map to the repository entity
            await _companyRepository.AddAsync(entity);
        }

        public async Task DeleteCompanyAsync(string companyCode)
        {
            var entity = await _companyRepository.GetByCodeAsync(companyCode);
            if (entity == null)
                throw new KeyNotFoundException("Company not found");

            await _companyRepository.DeleteAsync(entity);
        }

        public async Task UpdateCompanyAsync(CompanyInfo company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            var entity = _mapper.Map<Company>(company); // Map to the repository entity
            await _companyRepository.UpdateAsync(entity);
        }
    }
}

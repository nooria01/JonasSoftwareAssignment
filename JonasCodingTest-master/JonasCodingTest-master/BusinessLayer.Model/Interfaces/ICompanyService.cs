using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;


namespace BusinessLayer.Model.Interfaces
{
    public interface ICompanyService
    {/*
        IEnumerable<CompanyInfo> GetAllCompanies();
        CompanyInfo GetCompanyByCode(string companyCode);
        void AddCompany(CompanyInfo company);
        void UpdateCompany(CompanyInfo company);
        void DeleteCompany(string companyCode);*/


        Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync();
        Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode);
        Task AddCompanyAsync(CompanyInfo company);
        Task DeleteCompanyAsync(string companyCode);
        Task UpdateCompanyAsync(CompanyInfo company);
    }
}

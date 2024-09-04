using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface ICompanyRepository
    {/*
        IEnumerable<Company> GetAll();
        Company GetByCode(string companyCode);
        bool SaveCompany(Company company);

        void Add(Company entity);
        void Delete(Company entity);
        void Update(Company entity);*/


        Task<IEnumerable<Company>> GetAllAsync();
        Task<Company> GetByCodeAsync(string companyCode);
        Task<bool> SaveCompanyAsync(Company company);
        Task AddAsync(Company entity);
        Task DeleteAsync(Company entity);
        Task UpdateAsync(Company entity);
    }
}

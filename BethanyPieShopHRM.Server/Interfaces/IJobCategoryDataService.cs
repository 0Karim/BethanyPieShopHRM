using BethanysPieShopHRM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShopHRM.Server.Interfaces
{
    public interface IJobCategoryDataService
    {
        Task<IEnumerable<JobCategory>> GetAllJobCateory();
        Task<JobCategory> GetJobCategoryById(int countryId);
    }
}

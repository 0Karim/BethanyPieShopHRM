using BethanyPieShopHRM.Server.Interfaces;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShopHRM.Server.Pages
{
    public partial class EmployeeEdit : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { set; get; }

        [Inject]
        public ICountryDataService CountryDataService { set; get; }

        [Inject]
        public IJobCategoryDataService JobCategoryDataService { set; get; }

        [Inject]
        public NavigationManager NavigationManager { set; get; }

        [Parameter]
        public string EmployeeId { set; get; }

        public List<Country> Countries { set; get; } = new List<Country>();

        public List<JobCategory> JobCategories { set; get; } = new List<JobCategory>();

        protected Employee Employee { set; get; } = new Employee();
        protected string CountryId = string.Empty;
        protected string JobCategoryId = string.Empty;

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            int.TryParse(EmployeeId, out var employeeId);

            if (employeeId == 0)
            {
                Employee = new Employee { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };
            }
            else
            {
                Employee = await EmployeeDataService.GetEmployeeDetails(Convert.ToInt32(EmployeeId));
            }

            Countries = (await CountryDataService.GetAllCountries()).ToList();
            JobCategories = (await JobCategoryDataService.GetAllJobCateory()).ToList();
            CountryId = Employee.CountryId.ToString();
            JobCategoryId = Employee.JobCategoryId.ToString();
        }

        protected async Task HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors , please try again";
        }

        protected async Task HandleValidSubmit()
        {
            Saved = false;

            Employee.CountryId = int.Parse(CountryId);
            Employee.JobCategoryId = int.Parse(JobCategoryId);

            if(Employee.EmployeeId == 0)
            {
                var addedEmployee = await EmployeeDataService.AddEmployee(Employee);
                if (addedEmployee != null)
                {
                    StatusClass = "alert-success";
                    Message = "New employee added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new employee. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await EmployeeDataService.UpdateEmployee(Employee);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
            }
        }


        protected async Task DeleteEmployee()
        {
            await EmployeeDataService.DeleteEmployee(Employee.EmployeeId);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/employeeoverview");
        }

    }
}
    
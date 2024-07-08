using System;
using System.Threading.Tasks;
using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging; // tambahkan namespace logging

namespace EmployeeManagementWebApp.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly EmployeeService _employeeService;
        private readonly ILogger<DetailsModel> _logger; // tambahkan logger

        public DetailsModel(EmployeeService employeeService, ILogger<DetailsModel> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Employee = await _employeeService.GetByIdAsync(id);

                if (Employee == null)
                {
                    _logger.LogWarning($"Employee with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Employee found: {Employee.Nik}");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving employee with ID {id}");
                throw;
            }
        }
    }
}

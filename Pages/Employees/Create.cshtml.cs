using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.Services;
using System.Threading.Tasks;
using System;

namespace EmployeeManagementWebApp.Pages.Employees
{
   public class CreateModel : PageModel
   {
      private readonly EmployeeService _employeeService;

      [BindProperty]
      public Employee Employee { get; set; }

      public CreateModel(EmployeeService employeeService)
      {
         _employeeService = employeeService;
      }

      public IActionResult OnGet()
      {
         return Page();
      }

      public async Task<IActionResult> OnPostAsync()
      {
         if (!ModelState.IsValid)
         {
            return Page();
         }

         try
         {
            await _employeeService.CreateAsync(Employee);
            return RedirectToPage("./Index"); // Redirect to Index page after successful creation
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", $"Failed to create employee: {ex.Message}");
            return Page(); // Stay on the same page with error message
         }
      }
   }
}

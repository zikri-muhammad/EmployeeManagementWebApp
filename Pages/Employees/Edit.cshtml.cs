using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.Services;
using System.Threading.Tasks;
using System;

namespace EmployeeManagementWebApp.Pages.Employees
{
   public class EditModel : PageModel
   {
      private readonly EmployeeService _employeeService;

      [BindProperty]
      public Employee Employee { get; set; }

      public EditModel(EmployeeService employeeService)
      {
         _employeeService = employeeService;
      }

      public async Task<IActionResult> OnGetAsync(int id)
      {
         Employee = await _employeeService.GetByIdAsync(id);

         if (Employee == null)
         {
            return NotFound();
         }
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
            await _employeeService.UpdateAsync(Employee);
            return RedirectToPage("./Index");
         }
         catch (Exception ex)
         {
            ModelState.AddModelError("", $"Failed to update employee: {ex.Message}");
            return Page();
         }
      }
   }
}

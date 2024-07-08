using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementWebApp.Services; // Sesuaikan dengan namespace service Anda

namespace EmployeeManagementWebApp.Pages.Employees
{
   public class DeleteModel : PageModel
   {
      private readonly EmployeeService _employeeService;

      public DeleteModel(EmployeeService employeeService)
      {
         _employeeService = employeeService;
      }

      public async Task<IActionResult> OnPostDeleteAsync(int id)
      {
         Console.WriteLine($"Deleting employee with ID {id}");
         try
         {
            await _employeeService.DeleteAsync(id);
            return RedirectToPage("./Index");
         }
         catch (Exception ex)
         {
            // Handle any exceptions (e.g., log, display error message)
            ModelState.AddModelError(string.Empty, $"Failed to delete the employee: {ex.Message}");
            return Page();
         }
      }
   }
}

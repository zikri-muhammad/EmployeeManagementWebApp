using Microsoft.AspNetCore.Mvc.RazorPages;
using EmployeeManagementWebApp.Models;
using EmployeeManagementWebApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementWebApp.Pages.Employees
{
   public class IndexModel : PageModel
   {
      private readonly EmployeeService _employeeService;

      public IndexModel(EmployeeService employeeService)
      {
         _employeeService = employeeService;
      }

      public PagedResult<Employee> Employees { get; set; }
      [BindProperty(SupportsGet = true)]
      public string SearchNama { get; set; }
      [BindProperty(SupportsGet = true)]
      public string SearchDepartemen { get; set; }
      [BindProperty(SupportsGet = true)]
      public string SearchJenisKelamin { get; set; }
      [BindProperty(SupportsGet = true)]
      public string SearchJabatan { get; set; }
      [BindProperty(SupportsGet = true)]
      public DateTimeOffset? SearchTanggalLahir { get; set; }
      [BindProperty(SupportsGet = true)]
      public int PageNumber { get; set; } = 1;
      [BindProperty(SupportsGet = true)]
      public int PageSize { get; set; } = 10;

      public async Task OnGetAsync()
      {
         var query = new EmployeeSearchQuery
         {
            Nama = SearchNama,
            Departemen = SearchDepartemen,
            JenisKelamin = SearchJenisKelamin,
            Jabatan = SearchJabatan,
            TanggalLahir = SearchTanggalLahir,
            PageNumber = PageNumber,
            PageSize = PageSize
         };

         Employees = await _employeeService.SearchAsync(query);
      }

      public async Task<IActionResult> OnPostDeleteAsync(int id)
      {
         await _employeeService.DeleteAsync(id);
         return RedirectToPage();
      }
   }
}

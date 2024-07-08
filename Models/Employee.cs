using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWebApp.Models
{
   public class Employee
   {
      public int Id { get; set; }

      [RegularExpression(@"^\d{16}$", ErrorMessage = "Nik harus terdiri dari 16 digit angka.")]
      public string Nik { get; set; }
      public string Nama { get; set; }
      public string Alamat { get; set; }
      public DateTimeOffset TanggalLahir { get; set; }
      public string JenisKelamin { get; set; }
      public string Departemen { get; set; }
      public string Jabatan { get; set; }
   }

   public class PagedResult<T>
   {
      public List<T> Items { get; set; }
      public int TotalCount { get; set; }
      public int PageNumber { get; set; }
      public int PageSize { get; set; }
   }

   public class EmployeeSearchQuery
   {
      public string Nama { get; set; }
      public string Departemen { get; set; }
      public string JenisKelamin { get; set; }
      public DateTimeOffset? TanggalLahir { get; set; }
      public string Jabatan { get; set; }
      public int PageNumber { get; set; } = 1;
      public int PageSize { get; set; } = 10;
   }
}
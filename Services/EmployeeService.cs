using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EmployeeManagementWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace EmployeeManagementWebApp.Services
{
   public class EmployeeService
   {
      private readonly HttpClient _httpClient;
      private readonly IHttpContextAccessor _httpContextAccessor;

      public EmployeeService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
      {
         _httpClient = clientFactory.CreateClient("EmployeeApi");
         _httpClient.BaseAddress = new Uri("https://localhost:7028/api/");
         _httpContextAccessor = httpContextAccessor;
      }

      private void AddAuthorizationHeader()
      {
         var token = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
         if (!string.IsNullOrEmpty(token))
         {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         }
      }

      public async Task<PagedResult<Employee>> GetAllAsync()
      {
         AddAuthorizationHeader();
         try
         {
            var response = await _httpClient.GetAsync("employee");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PagedResult<Employee>>();
         }
         catch (HttpRequestException ex)
         {
            throw new Exception("Failed to retrieve employees", ex);
         }
      }

      public async Task<PagedResult<Employee>> SearchAsync(EmployeeSearchQuery query)
      {
         AddAuthorizationHeader();
         try
         {
            // Menyusun query string dengan benar
            var queryString = $"Nama={Uri.EscapeDataString(query.Nama ?? "")}&Departemen={Uri.EscapeDataString(query.Departemen ?? "")}&JenisKelamin={Uri.EscapeDataString(query.JenisKelamin ?? "")}&Jabatan={Uri.EscapeDataString(query.Jabatan ?? "")}&TanggalLahir={(query.TanggalLahir.HasValue ? Uri.EscapeDataString(query.TanggalLahir.Value.ToString("yyyy-MM-dd")) : "")}&PageNumber={query.PageNumber}&PageSize={query.PageSize}";

            var response = await _httpClient.GetAsync($"employee?{queryString}");

            // Debugging tambahan
            Console.WriteLine($"Request URL: {response.RequestMessage.RequestUri}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Body: {responseBody}");

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<PagedResult<Employee>>(responseBody, new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            });
         }
         catch (HttpRequestException ex)
         {
            throw new Exception("Failed to search employees", ex);
         }
      }


      public async Task<Employee> GetByIdAsync(int id)
      {
         AddAuthorizationHeader();
         try
         {
            var response = await _httpClient.GetAsync($"employee/{id}");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response data: {data}");

            var result = JsonSerializer.Deserialize<EmployeeResponse>(data, new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            });

            return result.Data; // Assuming the actual Employee object is stored in 'Data' property
         }
         catch (HttpRequestException ex)
         {
            throw new Exception($"Failed to retrieve employee with ID {id}", ex);
         }
      }

      public class EmployeeResponse
      {
         public bool Success { get; set; }
         public string Message { get; set; }
         public Employee Data { get; set; }
      }


      public async Task CreateAsync(Employee employee)
      {
         AddAuthorizationHeader();
         try
         {
            employee.TanggalLahir = employee.TanggalLahir.ToUniversalTime();
            var response = await _httpClient.PostAsJsonAsync("employee", employee);
            response.EnsureSuccessStatusCode();
         }
         catch (HttpRequestException ex)
         {
            throw new Exception("Failed to create employee", ex);
         }
      }

      public async Task UpdateAsync(Employee employee)
      {
         AddAuthorizationHeader();
         try
         {
            employee.TanggalLahir = employee.TanggalLahir.ToUniversalTime();
            var response = await _httpClient.PutAsJsonAsync($"employee/{employee.Id}", employee);
            response.EnsureSuccessStatusCode();
         }
         catch (HttpRequestException ex)
         {
            throw new Exception($"Failed to update employee with ID {employee.Id}", ex);
         }
      }
      public async Task DeleteAsync(int id)
      {

         AddAuthorizationHeader();
         Console.WriteLine($"Employee with ID {id} deleted successfully.");
         try
         {
            var response = await _httpClient.DeleteAsync($"employee/{id}");
            response.EnsureSuccessStatusCode();
         }
         catch (HttpRequestException ex)
         {
            Console.WriteLine($"Failed to delete employee with ID {id}: {ex.Message}");
            throw new Exception($"Failed to delete employee with ID {id}", ex);
         }
      }

   }
}

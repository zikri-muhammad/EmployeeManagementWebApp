using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System;

namespace EmployeeManagementWebApp.Pages.Account
{
   public class LoginModel : PageModel
   {
      private readonly HttpClient _httpClient;
      private readonly IConfiguration _configuration;

      public LoginModel(IHttpClientFactory clientFactory, IConfiguration configuration)
      {
         _httpClient = clientFactory.CreateClient("EmployeeApi");
         _configuration = configuration;
      }

      [BindProperty]
      public LoginModelInput Login { get; set; }

      public class LoginModelInput
      {
         public string UserName { get; set; }
         public string Password { get; set; }
      }

      public async Task<IActionResult> OnPostAsync()
      {
         var response = await _httpClient.PostAsJsonAsync("account/login", Login);

         if (!response.IsSuccessStatusCode)
         {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
         }

         var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
         var token = result.Token;

         var handler = new JwtSecurityTokenHandler();
         var jwtToken = handler.ReadJwtToken(token);

         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, Login.UserName),
            new Claim("access_token", token)
         };

         claims.AddRange(jwtToken.Claims);

         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
         var authProperties = new AuthenticationProperties
         {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
            IsPersistent = true
         };

         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

         return RedirectToPage("/Employees/Index");

      }

      public class LoginResponse
      {
         public string Token { get; set; }
         public DateTime Expiration { get; set; }
      }
   }
}

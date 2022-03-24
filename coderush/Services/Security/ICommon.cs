using coderush.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace coderush.Services.Security
{
    public interface ICommon
    {
        Task CreateDefaultSuperAdmin();

        List<String> GetAllRoles();

        List<ApplicationUser> GetAllMembers();

        ApplicationUser GetMemberByApplicationId(string applicationId);

        Task<ApplicationUser> CreateApplicationUser(ApplicationUser applicationUser, string password);

        Task<ApplicationUser> GetCurrentUserLogin(ClaimsPrincipal user);

        string GetCurrentUserLoginId(ClaimsPrincipal user);

        Task<Employee> GetCurrentEmployeeLogin(ClaimsPrincipal user);

        Task<string> GetCurrentEmployeeLoginId(ClaimsPrincipal user);
    }
}

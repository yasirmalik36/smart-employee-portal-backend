// Interfaces/ICommonService.cs
using SmartEmpAPI.Models;
using SmartEmpAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartEmpAPI.Interfaces
{
    public interface ICommonService
    {
        Task<(Response, List<Dictionary<string, object>>)> GetDropdownDataAsync(string Param);
    }
}
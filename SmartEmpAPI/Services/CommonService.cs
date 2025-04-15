// Services/CommonService.cs
using SmartEmpAPI.DAL;
using SmartEmpAPI.Models;
using SmartEmpAPI.DTOs;
using SmartEmpAPI.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SmartEmpAPI.Helpers;

namespace SmartEmpAPI.Services
{
    public class CommonService : ICommonService
    {
        private readonly DatabaseHelper _databaseHelper;

        public CommonService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<(Response, List<Dictionary<string, object>>)> GetDropdownDataAsync(string Param)
        {

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TableName", Param),
            };
            var (dataSet, response) = _databaseHelper.ExecuteSPWithGenericOutput("PRC_Get_Dynamic_NameValue", parameters.ToArray());

            var dropdownList = Helper.ConvertDataSetToDictionaryList(dataSet);


            return (response, dropdownList);


        }
    }
}
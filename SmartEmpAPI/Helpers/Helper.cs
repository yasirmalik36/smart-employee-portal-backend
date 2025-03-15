
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
namespace SmartEmpAPI.Helpers
{


    public static class Helper
    {
        public static List<Dictionary<string, object>> ConvertDataSetToDictionaryList(DataSet dataSet)
        {
            var list = new List<Dictionary<string, object>>();

            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return list; // Return empty list if there's no data

            DataTable table = dataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col] != DBNull.Value ? row[col] : null;
                }

                list.Add(dict);
            }

            return list;
        }


    }

}

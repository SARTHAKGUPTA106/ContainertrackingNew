using ContainerTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Xml;




namespace Containertracking.Controllers
{
    public class GetData
    {
        private readonly IConfiguration _configuration;
        public List<Dictionary<string, object>> GetDataAsJson(string query)
        {
            var result = new List<Dictionary<string, object>>();
            string connectionString = _configuration.GetConnectionString("ErpDatabase");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string is missing.");

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] =
                                reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }

                        result.Add(row);
                    }
                }
            }

            return result;
        }



    }
}

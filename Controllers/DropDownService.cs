using ContainerTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Containertracking.Controllers
{
    public class DropdownService
    {
        private readonly IConfiguration _configuration;
           
        public DropdownService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public List<object> GetDropdownList(string query)
        {
            List<object> dropdownItems = new List<object>();

            string connectionString = _configuration.GetConnectionString("ErpDatabase");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not configured or is empty.");
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dropdownItems.Add(new
                    {
                        Value = reader[0].ToString(),
                        Text = reader[1].ToString()
                    });
                }
            }
            return dropdownItems;
        }
    }

}

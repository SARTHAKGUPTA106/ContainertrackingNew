using ContainerTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace Containertracking.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly GetData _getData;

        
        public LoginController(IConfiguration configuration,GetData getData)
        {
            _configuration = configuration;
            _getData = getData;
        }

        public IActionResult Index()
        {
            return View();
        }
          

        public IActionResult AuthenticateUser(string username, string password)
        {
            string query = $"SELECT COUNT(1) AS UserCount FROM Users WHERE Username = @Username AND Password = @Password";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Username", SqlDbType.NVarChar) { Value = username },
                new SqlParameter("@Password", SqlDbType.NVarChar) { Value = password }
            };
            int userCount = 0;
            string connectionString = _configuration.GetConnectionString("ErpDatabase");
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                con.Open();
                userCount = (int)cmd.ExecuteScalar();
            }
            if (userCount == 1)
            {
                return Json(new { isAuthenticated = true });
            }
            else
            {
                return Json(new { isAuthenticated = false });
            }
        }


    }


}

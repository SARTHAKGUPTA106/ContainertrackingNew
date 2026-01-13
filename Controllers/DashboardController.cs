using ContainerTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Containertracking.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly DropdownService _dropDownService;

         public DashboardController(IConfiguration configuration , DropdownService dropdownService)
        {
            _configuration = configuration;
            _dropDownService = dropdownService;
        }
          
        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public async Task<IActionResult> SearchAction(string searchTerm)
        {     

            var shipments = new List<ShippingDetailsModel>();
            string connectionString = _configuration.GetConnectionString("ErpDatabase");

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                  
                    string storedProcedureName = "sp_ShippingDetails";

                    using (var cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; 
                                              
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@Action", "SearchOption");

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                shipments.Add(new ShippingDetailsModel
                                {
                                    Mode = reader["Mode"]?.ToString(),
                                    ReferenceNo = reader["ReferenceNo"]?.ToString(),
                                    TrackingNumber = reader["TrackingNumber"]?.ToString(),
                                    Carrier = reader["Carrier"]?.ToString(),
                                    ContainerNumbers = reader["ContainerNumbers"]?.ToString(),
                                    FactoryDispatchDate = ParseDate(reader["FactoryDispatchDate"]),
                                    OriginPOL = reader["OriginPOL"]?.ToString(),
                                    Destination = reader["Destination"]?.ToString(),
                                    PortOfDeparture = reader["PortOfDeparture"]?.ToString(),
                                    CurrentPosition = reader["CurrentPosition"]?.ToString(),
                                    PortOfDestination = reader["PortOfDestination"]?.ToString(),
                                    Status = reader["Status"]?.ToString(),
                                    ShippingTime = ParseDate(reader["ShippingTime"]),
                                    ETA = ParseDate(reader["ETA"]),
                                    ActualTime = ParseDate(reader["ActualTime"]),
                                    ArrivalDate = ParseDate(reader["ArrivalDate"]),
                                    DispatchDate = ParseDate(reader["DispatchDate"])
                                });
                            }
                        }
                    }
                }

                return Json(shipments);
            }
            catch (Exception ex)
            {
                // Log ex if needed
                return StatusCode(500, "Internal server error");
            }
        }

        private DateTime? ParseDate(object value)
        {
            if (value == DBNull.Value || value == null)
                return null;

            DateTime date;
            return DateTime.TryParse(value.ToString(), out date) ? date : null;
        }

        [HttpPost]
        public async Task<IActionResult> AddShipment([FromBody] ShippingDetailsModel model)
        {
            if (model == null)
                return BadRequest(new { message = "Invalid data." });
            try
            {
                string connectionString = _configuration.GetConnectionString("ErpDatabase");

                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand("sp_ShippingDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INSERT");
                        cmd.Parameters.AddWithValue("@Mode", model.Mode);
                        cmd.Parameters.AddWithValue("@ReferenceNo", model.ReferenceNo);
                        cmd.Parameters.AddWithValue("@TrackingNumber", model.TrackingNumber);
                        cmd.Parameters.AddWithValue("@Carrier", model.Carrier);
                        cmd.Parameters.AddWithValue("@ContainerNumbers", model.ContainerNumbers);
                        cmd.Parameters.AddWithValue("@FactoryDispatchDate",model.FactoryDispatchDate);
                        cmd.Parameters.AddWithValue("@OriginPOL", model.OriginPOL);
                        cmd.Parameters.AddWithValue("@Destination", model.Destination);
                        cmd.Parameters.AddWithValue("@ModeID", model.ModeID);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                return Json(new { message = "Shipment added successfully." });
            }
            catch (Exception ex)
            {            
                return StatusCode(500, new { message = "Error saving shipment." });
            }
        }

        public JsonResult DDlMode()
        {
            string connectionString = _configuration.GetConnectionString("ErpDatabase");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT ModeID, Mode FROM mode";
                var PartyList = _dropDownService.GetDropdownList(query);
                return Json(PartyList);
            }
        }

        [HttpGet]
        public async Task<IActionResult> StatuswiseData(string searchTerm)
        {
            var shipments = new List<ShippingDetailsModel>();
            string connectionString = _configuration.GetConnectionString("ErpDatabase");
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {

                    string storedProcedureName = "sp_ShippingDetails";

                    using (var cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        cmd.Parameters.AddWithValue("@Action", "StatusWisedata");

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                shipments.Add(new ShippingDetailsModel
                                {
                                    Mode = reader["Mode"]?.ToString(),
                                    ReferenceNo = reader["ReferenceNo"]?.ToString(),
                                    TrackingNumber = reader["TrackingNumber"]?.ToString(),
                                    Carrier = reader["Carrier"]?.ToString(),
                                    ContainerNumbers = reader["ContainerNumbers"]?.ToString(),
                                    FactoryDispatchDate = ParseDate(reader["FactoryDispatchDate"]),
                                    OriginPOL = reader["OriginPOL"]?.ToString(),
                                    Destination = reader["Destination"]?.ToString(),
                                    PortOfDeparture = reader["PortOfDeparture"]?.ToString(),
                                    CurrentPosition = reader["CurrentPosition"]?.ToString(),
                                    PortOfDestination = reader["PortOfDestination"]?.ToString(),
                                    Status = reader["Status"]?.ToString(),
                                    ShippingTime = ParseDate(reader["ShippingTime"]),
                                    ETA = ParseDate(reader["ETA"]),
                                    ActualTime = ParseDate(reader["ActualTime"]),
                                    ArrivalDate = ParseDate(reader["ArrivalDate"]),
                                    DispatchDate = ParseDate(reader["DispatchDate"])
                                });
                            }
                        }
                    }
                }

                return Json(shipments);
            }
            catch (Exception ex)
            {              
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> StatusCount()
        {
          
            var statusCounts = new Dictionary<string, int>();
                      
            string connectionString = _configuration.GetConnectionString("ErpDatabase");

            try
            {         
                using (var conn = new SqlConnection(connectionString))
                {               
                    string storedProcedureName = "sp_ShippingDetails";
                                   
                    using (var cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "Statuscountdata");
                                               
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                    
                            if (await reader.ReadAsync())
                            {                          
                                statusCounts["Arrived"] = reader["Arrived"] != DBNull.Value ? Convert.ToInt32(reader["Arrived"]) : 0;
                                statusCounts["Delayed"] = reader["Delayed"] != DBNull.Value ? Convert.ToInt32(reader["Delayed"]) : 0;
                                statusCounts["En_route"] = reader["En_route"] != DBNull.Value ? Convert.ToInt32(reader["En_route"]) : 0;
                                statusCounts["In_transit"] = reader["In_transit"] != DBNull.Value ? Convert.ToInt32(reader["In_transit"]) : 0;
                                statusCounts["Shipped"] = reader["Shipped"] != DBNull.Value ? Convert.ToInt32(reader["Shipped"]) : 0;
                                statusCounts["Total"] = reader["Total"] != DBNull.Value ? Convert.ToInt32(reader["Total"]) : 0;
                            }
                        }
                    }
                }
                              
                return Json(statusCounts);
            }
            catch (Exception ex)
            {            
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

    }
}

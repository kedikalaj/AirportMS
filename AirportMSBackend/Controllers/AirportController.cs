using AirportMS;
using AirportMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace AirportMS.Controllers
{
    public class AirportController : ApiControllerAttribute
    {
        private readonly string _connectionString;
        public AirportController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


    }
}
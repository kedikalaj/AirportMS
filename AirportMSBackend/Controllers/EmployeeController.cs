using AirportMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace AirportMS.Controllers
{
    public class EmployeeController : ApiControllerAttribute
    {
        private readonly string _connectionString;
        public EmployeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("Airport/GetEmployees")]
        public ActionResult<IEnumerable<Employee>> GetEmployee()
        {
            var items = new List<Employee>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string script = "SELECT  EMPLOYEE.EMPLOYEE_ID,   EMPLOYEE.EMPLOYEE_FNAME,   EMPLOYEE.EMPLOYEE_LNAME,  EMPLOYEE.EMPLOYEE_SALARY_BONUS,    ROLE.ROLE_ID,  ROLE.ROLE_DESCRIPTION,    ROLE.ROLE_BASE_SALARY FROM  EMPLOYEE JOIN     ROLE ON EMPLOYEE.ROLE_ID = ROLE.ROLE_ID JOIN   EMPLOYEE_AIRPORT ON EMPLOYEE.EMPLOYEE_ID = EMPLOYEE_AIRPORT.EMPLOYEE_ID where EMPLOYEE_AIRPORT.AIRPORT_ID = 3;";
                using (var command = new SqlCommand(script, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Employee
                        {
                            EMPLOYEE_ID = (int)reader["EMPLOYEE_ID"],
                            EMPLOYEE_SALARY_BONUS = (int)reader["EMPLOYEE_SALARY_BONUS"],
                            EMPLOYEE_LNAME = reader["EMPLOYEE_LNAME"].ToString(),
                            EMPLOYEE_FNAME = reader["EMPLOYEE_FNAME"].ToString(),
                            ROLE_BASE_SALARY = (int)reader["ROLE_BASE_SALARY"],
                            ROLE_DESCRIPTION = reader["ROLE_DESCRIPTION"].ToString(),
                            ROLE_ID = (int)reader["ROLE_ID"],
                        };

                        items.Add(item);
                    }
                }
            }
            return items;
        }

        [HttpPut("Airport/ModifyEmployee/{id}")]
        public IActionResult ModifyEmployee(int id, [FromBody] Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UPDATE EMPLOYEE SET ROLE_ID = @role, EMPLOYEE_FNAME = @fname, EMPLOYEE_LNAME=@lname, EMPLOYEE_SALARY_BONUS = @bonus WHERE EMPLOYEE_ID = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@role", employee.ROLE_ID);
                    command.Parameters.AddWithValue("@fname", employee.EMPLOYEE_FNAME);
                    command.Parameters.AddWithValue("@lname", employee.EMPLOYEE_LNAME);
                    command.Parameters.AddWithValue("@bonus", employee.EMPLOYEE_SALARY_BONUS);

                    var rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return new BadRequestObjectResult("Employee not found");
                    }
                    else
                    {
                        return new OkObjectResult("Success");
                    }
                }
            }
        }
        [HttpDelete("Airport/DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("delete from EMPLOYEE_AIRPORT WHERE EMPLOYEE_ID =  @Id; DELETE FROM EMPLOYEE WHERE EMPLOYEE_ID = @Id;", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    var rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return new BadRequestObjectResult("Employee not found");
                    }
                    else
                    {
                        return new OkObjectResult("Success");
                    }
                }
            }
        }
        [HttpPost("Airport/CreateEmployee")]
        public IActionResult CreateEmployee([FromBody] EmployeeCreate employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("INSERT INTO EMPLOYEE (ROLE_ID, EMPLOYEE_FNAME, EMPLOYEE_LNAME, EMPLOYEE_SALARY_BONUS)\r\nVALUES (@ROLE_ID, @EMPLOYEE_FNAME, @EMPLOYEE_LNAME, @EMPLOYEE_SALARY_BONUS); DECLARE @EmployeeID INT;\r\n" +
                    "SELECT @EmployeeID = EMPLOYEE_ID\r\nFROM EMPLOYEE\r\nWHERE EMPLOYEE_FNAME = '@EMPLOYEE_FNAME' AND @EMPLOYEE_LNAME = 'string'\r\n;\r\n\r\nINSERT INTO EMPLOYEE_AIRPORT (AIRPORT_ID, EMPLOYEE_ID)\r\nVALUES (3, @EmployeeID);\r\n;", connection))
                {
                    command.Parameters.AddWithValue("@ROLE_ID", employee.ROLE_ID);
                    command.Parameters.AddWithValue("@EMPLOYEE_FNAME", employee.EMPLOYEE_FNAME);
                    command.Parameters.AddWithValue("@EMPLOYEE_LNAME", employee.EMPLOYEE_LNAME);
                    command.Parameters.AddWithValue("@EMPLOYEE_SALARY_BONUS", employee.EMPLOYEE_SALARY_BONUS);

                    var rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return new BadRequestObjectResult("Employee not found");
                    }
                    else
                    {
                        return new OkObjectResult("Success");
                    }
                }
            }
        }
        /// <summary>
        /// Gets the average bonus per role
        /// </summary>
        /// <returns></returns>
        [HttpGet("Employee/AverageBonus")]
        public ActionResult<IEnumerable<AverageWage>> AverageBonus()
        {
            var items = new List<AverageWage>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string script = "\r\nSELECT r.ROLE_ID, r.ROLE_DESCRIPTION, AVG(e1.EMPLOYEE_SALARY_BONUS) AS AVG_BONUS\r\nFROM EMPLOYEE e1\r\nINNER JOIN EMPLOYEE e2 ON e1.ROLE_ID = e2.ROLE_ID AND e1.EMPLOYEE_ID <> e2.EMPLOYEE_ID\r\nINNER JOIN ROLE r ON e1.ROLE_ID = r.ROLE_ID\r\nGROUP BY r.ROLE_ID, r.ROLE_DESCRIPTION;\r\n";
                using (var command = new SqlCommand(script, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new AverageWage
                        {
                            RoleName = reader["ROLE_DESCRIPTION"].ToString(),
                            AverageRoleBonus = (int?)reader["AVG_BONUS"],
                        };

                        items.Add(item);
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Gets the numbers of employees with bonuses more than 50 and less than 5, based on role
        /// </summary>
        /// <returns></returns>
        [HttpGet("Employee/EmployeesWithBonus")]
        public ActionResult<IEnumerable<BonusByRole>> EmployeesWithBonus()
        {
            var items = new List<BonusByRole>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string script = "use AirportManage;\r\nSELECT ROLE_DESCRIPTION,\r\n       COUNT(CASE WHEN EMPLOYEE_SALARY_BONUS < 50 THEN EMPLOYEE_ID END) AS MoreThan50,\r\n       COUNT(CASE WHEN EMPLOYEE_SALARY_BONUS > 50 THEN EMPLOYEE_ID END) AS LessThan50\r\nFROM EMPLOYEE\r\nINNER JOIN ROLE ON EMPLOYEE.ROLE_ID = ROLE.ROLE_ID\r\nGROUP BY ROLE_DESCRIPTION;\r\n";
                using (var command = new SqlCommand(script, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new BonusByRole
                        {
                            RoleName = reader["ROLE_DESCRIPTION"].ToString(),
                            BonusOver50 = (int)reader["MoreThan50"],
                            BonusUnder50 = (int)reader["LessThan50"],
                        };

                        items.Add(item);
                    }
                }
            }
            return items;
        }
    }
}

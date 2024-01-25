namespace AirportMS.Models
{
    public class Employee
    {
        public int EMPLOYEE_ID { get; set; }
        public int EMPLOYEE_SALARY_BONUS { get; set; }
        public string EMPLOYEE_FNAME { get; set; }
        public string EMPLOYEE_LNAME { get; set; }
        public int ROLE_ID { get; set; }
        public string ROLE_DESCRIPTION { get; set; }
        public int ROLE_BASE_SALARY { get; set; }
    }
    public class EmployeeCreate
    {

        public int EMPLOYEE_SALARY_BONUS { get; set; }
        public string EMPLOYEE_FNAME { get; set; }
        public string EMPLOYEE_LNAME { get; set; }
        public int ROLE_ID { get; set; }
    }
    public class AverageWage
    {
        public string RoleName { get; set; }
        public int? AverageRoleBonus { get; set; }
    }
    public class BonusByRole
    {
        public string RoleName { get; set; }
        public int BonusOver50 { get; set; }
        public int BonusUnder50 { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Models
{
        public class Employee
    {
        public int EmployeeID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string EmailAddress { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public int Age => DateTime.Now.Year - DateOfBirth.Year;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public int DepartmentID { get; set; }

        public string? DeptName { get; set; }
    }
}

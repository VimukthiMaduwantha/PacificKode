using System.ComponentModel.DataAnnotations;

namespace CompanyManagement.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required]
        public string DepartmentCode { get; set; }

        [Required]
        public string DepartmentName { get; set; }
    }
}

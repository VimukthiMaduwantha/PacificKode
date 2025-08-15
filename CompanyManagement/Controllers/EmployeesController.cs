using CompanyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net.Mail;

namespace CompanyManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _config;

        public EmployeesController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            var Employee = new List<Employee>();
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("SELECT E.EmpID, E.FName, E.LName, E.Email, E.DOB, E.Age, E.Salary, E.DepartmentID, D.DeptName FROM Employee E INNER JOIN Department D ON D.DeptID = E.DepartmentID", con);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Employee.Add(new Employee
                {
                    EmployeeID = (int)reader["EmpID"],
                    FirstName = reader["FName"].ToString(),
                    LastName = reader["LName"].ToString(),
                    EmailAddress = reader["Email"].ToString(),
                    DateOfBirth = reader["DOB"] != DBNull.Value
                    ? Convert.ToDateTime(reader["DOB"])
                    : default(DateTime),
                    Salary = (decimal)reader["Salary"],
                    DepartmentID = (int)reader["DepartmentID"],
                    DeptName = reader["DeptName"].ToString()
                });
            }
            return View(Employee);
        }

        public IActionResult Create()
        {
            var employee = new Employee();
            ViewBag.Departments = GetDepartments();
            return View(employee);
        }

        private List<Department> GetDepartments()
        {
            var list = new List<Department>();
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("SELECT DeptID, DeptName FROM Department", con);
            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Department
                {
                    DepartmentID = Convert.ToInt32(reader["DeptID"]),
                    DepartmentName = reader["DeptName"].ToString()
                });
            }
            return list;
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("INSERT INTO Employee (FName, LName, Email, DOB, Age, Salary, DepartmentID) VALUES (@fname, @lname, @email, @dob, @age, @salary, @deptID)", con);
            cmd.Parameters.AddWithValue("@fname", employee.FirstName);
            cmd.Parameters.AddWithValue("@lname", employee.LastName);
            cmd.Parameters.AddWithValue("@email", employee.EmailAddress);
            cmd.Parameters.AddWithValue("@dob", employee.DateOfBirth);
            cmd.Parameters.AddWithValue("@age", employee.Age);
            cmd.Parameters.AddWithValue("@salary", employee.Salary);
            cmd.Parameters.AddWithValue("@deptID", employee.DepartmentID);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpGet("/Employees/Create/{EmpID}")]
        public IActionResult Create(int EmpID)
        {
            ViewBag.Departments = GetDepartments();
            Employee emp = new();
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("SELECT E.EmpID, E.FName, E.LName, E.Email, E.DOB, E.Age, E.Salary, E.DepartmentID, D.DeptName FROM Employee E INNER JOIN Department D ON D.DeptID = E.DepartmentID WHERE EmpID=@id", con);
            cmd.Parameters.AddWithValue("@id", EmpID);
            con.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                emp.EmployeeID = (int)reader["EmpID"];
                emp.FirstName = reader["FName"].ToString();
                emp.LastName = reader["LName"].ToString();
                emp.EmailAddress = reader["Email"].ToString();
                emp.DateOfBirth = reader["DOB"] != DBNull.Value
                ? Convert.ToDateTime(reader["DOB"])
                : default(DateTime);
                emp.Salary = (decimal)reader["Salary"];
                emp.DepartmentID = (int)reader["DepartmentID"];
                emp.DeptName = reader["DeptName"].ToString();
            }
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (!ModelState.IsValid)
                return View(employee);
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("UPDATE Employee SET FName=@fname, LName=@lname, Email = @email, DOB = @dob, Age = @age, Salary = @salary, DepartmentID = @deptID WHERE EmpID=@id", con);
            cmd.Parameters.AddWithValue("@fname", employee.FirstName);
            cmd.Parameters.AddWithValue("@lname", employee.LastName);
            cmd.Parameters.AddWithValue("@email", employee.EmailAddress);
            cmd.Parameters.AddWithValue("@dob", employee.DateOfBirth);
            cmd.Parameters.AddWithValue("@age", employee.Age);
            cmd.Parameters.AddWithValue("@salary", employee.Salary);
            cmd.Parameters.AddWithValue("@deptID", employee.DepartmentID);
            cmd.Parameters.AddWithValue("@id", employee.EmployeeID);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("DELETE FROM Employee WHERE EmpID=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }
    }
}

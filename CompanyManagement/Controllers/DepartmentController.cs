using CompanyManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection;

namespace CompanyManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _config;

        public DepartmentController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            var dept = new List<Department>();
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("SELECT * FROM Department", con);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dept.Add(new Department
                {
                    DepartmentID = (int)reader["DeptID"],
                    DepartmentCode = reader["DeptCode"].ToString(),
                    DepartmentName = reader["DeptName"].ToString(),
                });
            }
            return View(dept);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
                return View(department);

            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("INSERT INTO Department (DeptCode, DeptName) VALUES (@DeptCode, @DeptName)", con);
            cmd.Parameters.AddWithValue("@DeptCode", department.DepartmentCode);
            cmd.Parameters.AddWithValue("@DeptName", department.DepartmentName);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpGet("Department/Create/{DeptID}")]
        public IActionResult Create(int DeptID)
        {
            Department dept = new();
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("SELECT * FROM Department WHERE DeptID=@id", con);
            cmd.Parameters.AddWithValue("@id", DeptID);
            con.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                dept.DepartmentID = (int)reader["DeptID"];
                dept.DepartmentCode = reader["DeptCode"].ToString();
                dept.DepartmentName = reader["DeptName"].ToString();
            }
            return View(dept);
        }

        [HttpPost]
        public IActionResult Edit(Department model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("UPDATE Department SET DeptCode=@DepartmentCode, DeptName=@DepartmentName WHERE DeptID=@id", con);
            cmd.Parameters.AddWithValue("@DepartmentCode", model.DepartmentCode);
            cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
            cmd.Parameters.AddWithValue("@id", model.DepartmentID);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var cmd = new SqlCommand("DELETE FROM Department WHERE DeptID=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

    }
}

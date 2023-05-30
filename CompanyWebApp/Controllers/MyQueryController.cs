using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompanyWebApp.Models;
using System.Data.SqlClient;


namespace CompanyWebApp.Controllers
{
    public class MyQueryController : Controller
    {
        public const string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=ITCompanyDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        // GET: MyQueryController
        public IActionResult Index()
        {
            return View(new MyQueryViewModel());
        }

        [HttpPost]
        public IActionResult Execute1(MyQueryViewModel model)
        {
            string countryName = model.ParameterStringValue;
            string query = "SELECT c.Name AS CompanyName, ct.FullName AS ProgrammerName " +
                   "FROM Companies c " +
                   "INNER JOIN Programmers p ON c.Id = p.CompanyId " +
                   "INNER JOIN Citizens ct ON p.CitizenId = ct.Id " +
                   "INNER JOIN Countries co ON ct.CountryId = co.Id " +
                   "WHERE co.Name = @countryName";

            List<CompanyProgrammerViewModel> result = new List<CompanyProgrammerViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@countryName", countryName);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CompanyProgrammerViewModel item = new CompanyProgrammerViewModel();
                    item.CompanyName = reader["CompanyName"].ToString();
                    item.ProgrammerName = reader["ProgrammerName"].ToString();

                    result.Add(item);
                }

                reader.Close();
            }

            return View(result);
        }

        [HttpPost]
        public IActionResult Execute2(MyQueryViewModel model)
        {
            int? experienceThreshold = model.ParameterIntValue;
            string query = "SELECT DISTINCT co.Name AS CountryName " +
                   "FROM Countries co " +
                   "INNER JOIN Citizens ct ON co.Id = ct.CountryId " +
                   "INNER JOIN Programmers p ON ct.Id = p.CitizenId " +
                   "WHERE p.YearsExperience > @experienceThreshold";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@experienceThreshold", experienceThreshold);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Отримання результатів запиту
                List<string> countryNames = new List<string>();
                while (reader.Read())
                {
                    string countryName = (string)reader["CountryName"];
                    countryNames.Add(countryName);
                }

                return View(countryNames);
            }
        }

        [HttpPost]
        public IActionResult Execute3(MyQueryViewModel model)
        {
            List<Query3ResultViewModel> results = new List<Query3ResultViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string releaseDateThreshold = model.ParameterStringValue;
                connection.Open();
                string query3 = "SELECT pr.Name AS ProductName, c.Name AS CompanyName " +
                "FROM Products pr " +
                "INNER JOIN Companies c ON pr.CompanyId = c.Id " +
                "WHERE pr.ReleaseDate > @releaseDateThreshold";
                using (SqlCommand command = new SqlCommand(query3, connection))
                {
                    command.Parameters.AddWithValue("@releaseDateThreshold", releaseDateThreshold);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            string companyName = reader.GetString(1);

                            Query3ResultViewModel result = new Query3ResultViewModel
                            {
                                ProductName = productName,
                                CompanyName = companyName
                            };

                            results.Add(result);
                        }
                    }
                }
            }

            return View(results);
        }

        [HttpPost]
        public IActionResult Execute4(MyQueryViewModel model)
        {
            var employeeCountThreshold = model.ParameterIntValue;
            List<Query4ResultViewModel> results = new List<Query4ResultViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT ct.FullName AS ProgrammerName, p.Specialization, c.Name AS CompanyName, c.StaffCount AS EmployeeCount
                        FROM Programmers p
                        INNER JOIN Citizens ct ON p.CitizenId = ct.Id
                        INNER JOIN Companies c ON p.CompanyId = c.Id
                        Where c.StaffCount > @employeeCountThreshold";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@employeeCountThreshold", employeeCountThreshold);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Query4ResultViewModel result = new Query4ResultViewModel();
                            result.ProgrammerName = reader["ProgrammerName"].ToString();
                            result.Specialization = reader["Specialization"].ToString();
                            result.CompanyName = reader["CompanyName"].ToString();
                            result.EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]);

                            results.Add(result);
                        }
                    }
                }
            }

            return View(results);
        }

        [HttpPost]
        public IActionResult Execute5(MyQueryViewModel model)
        {
            var productCount = model.ParameterIntValue;
            List<Query5ResultViewModel> queryResults = new List<Query5ResultViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT c.Name AS CompanyName, p.Name AS ProductName, co.Name AS CountryName " +
                               "FROM Countries co " +
                               "INNER JOIN Companies c ON co.Id = c.CountryId " +
                               "INNER JOIN Products p ON c.Id = p.CompanyId " +
                               "GROUP BY co.Name, c.Name, p.Name " +
                               "HAVING COUNT(p.Id) >= @productCount";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productCount", productCount);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string companyName = reader.GetString(0);
                            string productName = reader.GetString(1);
                            string countryName = reader.GetString(2);
                            Query5ResultViewModel result = new Query5ResultViewModel
                            {
                                CompanyName = companyName,
                                ProductName = productName,
                                CountryName = countryName   
                            };

                            queryResults.Add(result);
                        }
                    }
                }
            }

            return View(queryResults);
        }

        [HttpPost]
        public IActionResult Execute6(MyQueryViewModel model)
        {
            var enteredId = model.ParameterIntValue;
            string query = "SELECT DISTINCT ct.FullName AS ProgrammerName " +
                   "FROM Programmers p " +
                   "INNER JOIN Companies c ON p.CompanyId = c.Id " +
                   "INNER JOIN Citizens ct ON p.CitizenId = ct.Id " +
                   "WHERE c.Id IN (SELECT CompanyId " +
                                  "FROM Programmers " +
                                  "WHERE CitizenId = @enteredId)";
            List<Query6ResultViewModel> programmers = new List<Query6ResultViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@enteredId", enteredId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Query6ResultViewModel result = new Query6ResultViewModel();
                            result.ProgrammerName = reader["ProgrammerName"].ToString();
                            programmers.Add(result);
                        }
                    }
                }
            }

            return View(programmers);
        }

        [HttpPost]
        public IActionResult Execute7()
        {
            List<string> countries = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT DISTINCT co.Name AS CountryName " +
                               "FROM Countries co " +
                               "WHERE NOT EXISTS ( " +
                               "    SELECT * " +
                               "    FROM Citizens ct " +
                               "    WHERE ct.CountryId = co.Id " +
                               "        AND ct.Id NOT IN ( " +
                               "            SELECT p.CitizenId " +
                               "            FROM Programmers p " +
                               "        ) " +
                               ")";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string countryName = reader["CountryName"].ToString();
                            countries.Add(countryName);
                        }
                    }
                }
            }

            return View(countries);
        }

        [HttpPost]
        public IActionResult Execute8()
        {
            List<Query8ResultViewModel> results = new List<Query8ResultViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT DISTINCT ct.FullName AS ProgrammerName, c.Name AS CompanyName " +
                   "FROM Citizens ct " +
                   "INNER JOIN Programmers p ON ct.Id = p.CitizenId " +
                   "INNER JOIN Companies c ON p.CompanyId = c.Id " +
                   "WHERE EXISTS (" +
                   "SELECT * " +
                   "FROM Programmers p1 " +
                   "WHERE p1.CompanyId = c.Id " +
                   "AND p1.Id <> p.Id)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Query8ResultViewModel result = new Query8ResultViewModel();
                            result.ProgrammerName = reader["ProgrammerName"].ToString();
                            result.CompanyName = reader["CompanyName"].ToString();
                            results.Add(result);
                        }
                    }
                }
            }

            return View(results);
        }
    }
}

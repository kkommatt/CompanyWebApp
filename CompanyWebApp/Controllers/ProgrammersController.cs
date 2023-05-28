using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanyWebApp.Models;
using System.Data.SqlClient;

namespace CompanyWebApp.Controllers
{
    public class ProgrammersController : Controller
    {
        private readonly ItcompanyDbContext _context;

        public const string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=ITCompanyDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ProgrammersController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Programmers
        public async Task<IActionResult> Index()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.Id, p.Specialization, p.CitizenId, c.FullName AS CitizenName, p.CompanyId, co.Name AS CompanyName, p.YearsExperience, p.Range, p.Language, p.Salary, p.Time, p.Place FROM Programmers p INNER JOIN Citizens c ON p.CitizenId = c.Id INNER JOIN Companies co ON p.CompanyId = co.Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Programmer> programmers = new List<Programmer>();

                        while (reader.Read())
                        {
                            Programmer programmer = new Programmer
                            {
                                Id = (int)reader["Id"],
                                Specialization = (string)reader["Specialization"],
                                CitizenId = (int)reader["CitizenId"],
                                Citizen = new Citizen
                                {
                                    Id = (int)reader["CitizenId"],
                                    FullName = (string)reader["CitizenName"]
                                },
                                CompanyId = (int)reader["CompanyId"],
                                Company = new Company
                                {
                                    Id = (int)reader["CompanyId"],
                                    Name = (string)reader["CompanyName"]
                                },
                                YearsExperience = (int)reader["YearsExperience"],
                                Range = (string)reader["Range"],
                                Language = (string)reader["Language"],
                                Salary = (int)reader["Salary"],
                                Time = (string)reader["Time"],
                                Place = (string)reader["Place"]
                            };

                            programmers.Add(programmer);
                        }

                        return View(programmers);
                    }
                }
            }

        }
        /*
        // GET: Programmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Programmers == null)
            {
                return NotFound();
            }

            var programmer = await _context.Programmers
                .Include(p => p.Citizen)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (programmer == null)
            {
                return NotFound();
            }

            return View(programmer);
        }
        */
        // GET: Programmers/Create
        public IActionResult Create()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ViewData["CitizenId"] = GetCitizensSelectList(connection);
                ViewData["CompanyId"] = GetCompaniesSelectList(connection);
                return View();
            }
        }
        private SelectList GetCitizensSelectList(SqlConnection connection)
        {
            string query = "SELECT Id, FullName FROM Citizens";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<SelectListItem> items = new List<SelectListItem>();

                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string fullName = (string)reader["FullName"];
                    items.Add(new SelectListItem { Value = id.ToString(), Text = fullName });
                }

                return new SelectList(items, "Value", "Text");
            }
        }

        // Helper method to retrieve the SelectList for Companies
        private SelectList GetCompaniesSelectList(SqlConnection connection)
        {
            string query = "SELECT Id, Name FROM Companies";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                SqlDataReader reader = command.ExecuteReader();

                List<SelectListItem> items = new List<SelectListItem>();

                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string name = (string)reader["Name"];
                    items.Add(new SelectListItem { Value = id.ToString(), Text = name });
                }

                return new SelectList(items, "Value", "Text");
            }
        }

        // POST: Programmers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Specialization,CitizenId,CompanyId,YearsExperience,Range,Language,Salary,Time,Place")] Programmer programmer)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO Programmers (Specialization, CitizenId, CompanyId, YearsExperience, Range, Language, Salary, Time, Place) " +
                                   "VALUES (@Specialization, @CitizenId, @CompanyId, @YearsExperience, @Range, @Language, @Salary, @Time, @Place)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Specialization", programmer.Specialization);
                        command.Parameters.AddWithValue("@CitizenId", programmer.CitizenId);
                        command.Parameters.AddWithValue("@CompanyId", programmer.CompanyId);
                        command.Parameters.AddWithValue("@YearsExperience", programmer.YearsExperience);
                        command.Parameters.AddWithValue("@Range", programmer.Range);
                        command.Parameters.AddWithValue("@Language", programmer.Language);
                        command.Parameters.AddWithValue("@Salary", programmer.Salary);
                        command.Parameters.AddWithValue("@Time", programmer.Time);
                        command.Parameters.AddWithValue("@Place", programmer.Place);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string citizensQuery = "SELECT Id, FullName FROM Citizens";
                string companiesQuery = "SELECT Id, Name FROM Companies";

                using (SqlCommand citizensCommand = new SqlCommand(citizensQuery, connection))
                using (SqlCommand companiesCommand = new SqlCommand(companiesQuery, connection))
                {
                    List<SelectListItem> citizensList = new List<SelectListItem>();
                    List<SelectListItem> companiesList = new List<SelectListItem>();

                    using (SqlDataReader citizensReader = await citizensCommand.ExecuteReaderAsync())
                    {
                        while (citizensReader.Read())
                        {
                            int citizenId = (int)citizensReader["Id"];
                            string fullName = (string)citizensReader["FullName"];

                            SelectListItem item = new SelectListItem
                            {
                                Value = citizenId.ToString(),
                                Text = fullName
                            };

                            if (citizenId == programmer.CitizenId)
                            {
                                item.Selected = true;
                            }

                            citizensList.Add(item);
                        }
                    }

                    using (SqlDataReader companiesReader = await companiesCommand.ExecuteReaderAsync())
                    {
                        while (companiesReader.Read())
                        {
                            int companyId = (int)companiesReader["Id"];
                            string companyName = (string)companiesReader["Name"];

                            SelectListItem item = new SelectListItem
                            {
                                Value = companyId.ToString(),
                                Text = companyName
                            };

                            if (companyId == programmer.CompanyId)
                            {
                                item.Selected = true;
                            }

                            companiesList.Add(item);
                        }
                    }

                    ViewData["CitizenId"] = new SelectList(citizensList, "Value", "Text");
                    ViewData["CompanyId"] = new SelectList(companiesList, "Value", "Text");
                }
            }

            return View(programmer);
        }

        // GET: Programmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Programmer programmer = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Programmers WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            programmer = new Programmer
                            {
                                Id = (int)reader["Id"],
                                Specialization = (string)reader["Specialization"],
                                CitizenId = (int)reader["CitizenId"],
                                CompanyId = (int)reader["CompanyId"],
                                YearsExperience = (int)reader["YearsExperience"],
                                Range = (string)reader["Range"],
                                Language = (string)reader["Language"],
                                Salary = (int)reader["Salary"],
                                Time = (string)reader["Time"],
                                Place = (string)reader["Place"]
                            };
                        }
                    }
                }
            }

            if (programmer == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string citizensQuery = "SELECT Id, FullName FROM Citizens";
                string companiesQuery = "SELECT Id, Name FROM Companies";

                using (SqlCommand citizensCommand = new SqlCommand(citizensQuery, connection))
                using (SqlCommand companiesCommand = new SqlCommand(companiesQuery, connection))
                {
                    List<SelectListItem> citizensList = new List<SelectListItem>();
                    List<SelectListItem> companiesList = new List<SelectListItem>();

                    using (SqlDataReader citizensReader = await citizensCommand.ExecuteReaderAsync())
                    {
                        while (citizensReader.Read())
                        {
                            int citizenId = (int)citizensReader["Id"];
                            string fullName = (string)citizensReader["FullName"];

                            SelectListItem item = new SelectListItem
                            {
                                Value = citizenId.ToString(),
                                Text = fullName
                            };

                            if (citizenId == programmer.CitizenId)
                            {
                                item.Selected = true;
                            }

                            citizensList.Add(item);
                        }
                    }

                    using (SqlDataReader companiesReader = await companiesCommand.ExecuteReaderAsync())
                    {
                        while (companiesReader.Read())
                        {
                            int companyId = (int)companiesReader["Id"];
                            string companyName = (string)companiesReader["Name"];

                            SelectListItem item = new SelectListItem
                            {
                                Value = companyId.ToString(),
                                Text = companyName
                            };

                            if (companyId == programmer.CompanyId)
                            {
                                item.Selected = true;
                            }

                            companiesList.Add(item);
                        }
                    }

                    ViewData["CitizenId"] = new SelectList(citizensList, "Value", "Text");
                    ViewData["CompanyId"] = new SelectList(companiesList, "Value", "Text");
                }
            }

            return View(programmer);

        }

        // POST: Programmers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Specialization,CitizenId,CompanyId,YearsExperience,Range,Language,Salary,Time,Place")] Programmer programmer)
        {
            if (id != programmer.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string updateQuery = "UPDATE Programmers SET Specialization = @Specialization, CitizenId = @CitizenId, " +
                                        "CompanyId = @CompanyId, YearsExperience = @YearsExperience, Range = @Range, " +
                                        "Language = @Language, Salary = @Salary, Time = @Time, Place = @Place " +
                                        "WHERE Id = @Id";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Specialization", programmer.Specialization);
                        updateCommand.Parameters.AddWithValue("@CitizenId", programmer.CitizenId);
                        updateCommand.Parameters.AddWithValue("@CompanyId", programmer.CompanyId);
                        updateCommand.Parameters.AddWithValue("@YearsExperience", programmer.YearsExperience);
                        updateCommand.Parameters.AddWithValue("@Range", programmer.Range);
                        updateCommand.Parameters.AddWithValue("@Language", programmer.Language);
                        updateCommand.Parameters.AddWithValue("@Salary", programmer.Salary);
                        updateCommand.Parameters.AddWithValue("@Time", programmer.Time);
                        updateCommand.Parameters.AddWithValue("@Place", programmer.Place);
                        updateCommand.Parameters.AddWithValue("@Id", programmer.Id);

                        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                        if (rowsAffected == 0)
                        {
                            return NotFound();
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CitizenId"] = new SelectList(_context.Citizens, "Id", "FullName", programmer.CitizenId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", programmer.CompanyId);
            return View(programmer);
        }

        // GET: Programmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Programmer programmer = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string selectQuery = "SELECT p.Id, p.Specialization, p.CitizenId, p.CompanyId, p.YearsExperience, " +
                                     "p.Range, p.Language, p.Salary, p.Time, p.Place, c.FullName, cmp.Name " +
                                     "FROM Programmers p " +
                                     "JOIN Citizens c ON p.CitizenId = c.Id " +
                                     "JOIN Companies cmp ON p.CompanyId = cmp.Id " +
                                     "WHERE p.Id = @Id";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await selectCommand.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            programmer = new Programmer
                            {
                                Id = (int)reader["Id"],
                                Specialization = (string)reader["Specialization"],
                                CitizenId = (int)reader["CitizenId"],
                                CompanyId = (int)reader["CompanyId"],
                                YearsExperience = (int)reader["YearsExperience"],
                                Range = (string)reader["Range"],
                                Language = (string)reader["Language"],
                                Salary = (int)reader["Salary"],
                                Time = (string)reader["Time"],
                                Place = (string)reader["Place"],
                                Citizen = new Citizen
                                {
                                    Id = (int)reader["CitizenId"],
                                    FullName = (string)reader["FullName"]
                                },
                                Company = new Company
                                {
                                    Id = (int)reader["CompanyId"],
                                    Name = (string)reader["Name"]
                                }
                            };
                        }
                    }
                }
            }

            if (programmer == null)
            {
                return NotFound();
            }

            return View(programmer);

        }

        // POST: Programmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Programmers == null)
            {
                return Problem("Entity set 'ItcompanyDbContext.Programmers' is null.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string deleteQuery = "DELETE FROM Programmers WHERE Id = @Id";

                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammerExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Programmers WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}

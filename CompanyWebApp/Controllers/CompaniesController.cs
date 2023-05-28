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
    public class CompaniesController : Controller
    {
        private readonly ItcompanyDbContext _context;

        public const string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=ITCompanyDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public CompaniesController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Companies.Id, Companies.Name, Companies.City, Companies.Street, Companies.Header, " +
                               "Companies.StaffCount, Companies.CountryId, Companies.Website, Companies.Email, Companies.Edrpou, " +
                               "Countries.Name AS CountryName " +
                               "FROM Companies " +
                               "INNER JOIN Countries ON Companies.CountryId = Countries.Id";

                SqlCommand command = new SqlCommand(query, connection);
                List<Company> companies = new List<Company>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int companyId = reader.GetInt32(0);
                        string companyName = reader.GetString(1);
                        string city = reader.GetString(2);
                        string street = reader.GetString(3);
                        string header = reader.GetString(4);
                        int staffCount = reader.GetInt32(5);
                        int countryId = reader.GetInt32(6);
                        string website = reader.GetString(7);
                        string email = reader.GetString(8);
                        int edrpou = reader.GetInt32(9);
                        string countryName = reader.GetString(10);

                        Company company = new Company
                        {
                            Id = companyId,
                            Name = companyName,
                            City = city,
                            Street = street,
                            Header = header,
                            StaffCount = staffCount,
                            CountryId = countryId,
                            Website = website,
                            Email = email,
                            Edrpou = edrpou,
                            Country = new Country
                            {
                                Id = countryId,
                                Name = countryName
                            }
                        };

                        companies.Add(company);
                    }

                    reader.Close();
                }
                catch (Exception)
                {
                    throw;
                }

                return View(companies);
            }

        }
        /*
        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
        */
        // GET: Companies/Create
        public IActionResult Create()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Countries";

                SqlCommand command = new SqlCommand(query, connection);

                List<SelectListItem> countryList = new List<SelectListItem>();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int countryId = reader.GetInt32(0);
                        string countryName = reader.GetString(1);

                        SelectListItem countryItem = new SelectListItem
                        {
                            Value = countryId.ToString(),
                            Text = countryName
                        };

                        countryList.Add(countryItem);
                    }

                    reader.Close();
                }
                catch (Exception)
                {
                    throw;
                }

                ViewData["CountryId"] = new SelectList(countryList, "Value", "Text");
                return View();
            }

        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,Street,Header,StaffCount,CountryId,Website,Email,Edrpou")] Company company)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Companies (Name, City, Street, Header, StaffCount, CountryId, Website, Email, Edrpou) " +
                                   "VALUES (@Name, @City, @Street, @Header, @StaffCount, @CountryId, @Website, @Email, @Edrpou)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@City", company.City);
                    command.Parameters.AddWithValue("@Street", company.Street);
                    command.Parameters.AddWithValue("@Header", company.Header);
                    command.Parameters.AddWithValue("@StaffCount", company.StaffCount);
                    command.Parameters.AddWithValue("@CountryId", company.CountryId);
                    command.Parameters.AddWithValue("@Website", company.Website);
                    command.Parameters.AddWithValue("@Email", company.Email);
                    command.Parameters.AddWithValue("@Edrpou", company.Edrpou);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string countryQuery = "SELECT Id, Name FROM Countries";
                SqlCommand countryCommand = new SqlCommand(countryQuery, connection);

                await connection.OpenAsync();

                using (SqlDataReader reader = await countryCommand.ExecuteReaderAsync())
                {
                    List<SelectListItem> countries = new List<SelectListItem>();

                    while (reader.Read())
                    {
                        int countryId = reader.GetInt32(0);
                        string countryName = reader.GetString(1);
                        countries.Add(new SelectListItem { Value = countryId.ToString(), Text = countryName });
                    }

                    ViewData["CountryId"] = new SelectList(countries, "Value", "Text", company.CountryId);
                }
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Company company = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name, City, Street, Header, StaffCount, CountryId, Website, Email, Edrpou FROM Companies WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        company = new Company
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            City = reader.GetString(2),
                            Street = reader.GetString(3),
                            Header = reader.GetString(4),
                            StaffCount = reader.GetInt32(5),
                            CountryId = reader.GetInt32(6),
                            Website = reader.GetString(7),
                            Email = reader.GetString(8),
                            Edrpou = reader.GetInt32(9)
                        };
                    }
                }
            }

            if (company == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string countryQuery = "SELECT Id, Name FROM Countries";
                SqlCommand countryCommand = new SqlCommand(countryQuery, connection);

                await connection.OpenAsync();

                using (SqlDataReader reader = await countryCommand.ExecuteReaderAsync())
                {
                    List<SelectListItem> countries = new List<SelectListItem>();

                    while (reader.Read())
                    {
                        int countryId = reader.GetInt32(0);
                        string countryName = reader.GetString(1);
                        countries.Add(new SelectListItem { Value = countryId.ToString(), Text = countryName });
                    }

                    ViewData["CountryId"] = new SelectList(countries, "Value", "Text", company.CountryId);
                }
            }

            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,City,Street,Header,StaffCount,CountryId,Website,Email,Edrpou")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Companies SET Name = @Name, City = @City, Street = @Street, Header = @Header, StaffCount = @StaffCount, CountryId = @CountryId, Website = @Website, Email = @Email, Edrpou = @Edrpou WHERE Id = @Id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", company.Id);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@City", company.City);
                    command.Parameters.AddWithValue("@Street", company.Street);
                    command.Parameters.AddWithValue("@Header", company.Header);
                    command.Parameters.AddWithValue("@StaffCount", company.StaffCount);
                    command.Parameters.AddWithValue("@CountryId", company.CountryId);
                    command.Parameters.AddWithValue("@Website", company.Website);
                    command.Parameters.AddWithValue("@Email", company.Email);
                    command.Parameters.AddWithValue("@Edrpou", company.Edrpou);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string countryQuery = "SELECT Id, Name FROM Countries";
                SqlCommand countryCommand = new SqlCommand(countryQuery, connection);

                await connection.OpenAsync();

                using (SqlDataReader reader = await countryCommand.ExecuteReaderAsync())
                {
                    List<SelectListItem> countries = new List<SelectListItem>();

                    while (reader.Read())
                    {
                        int countryId = reader.GetInt32(0);
                        string countryName = reader.GetString(1);
                        countries.Add(new SelectListItem { Value = countryId.ToString(), Text = countryName });
                    }

                    ViewData["CountryId"] = new SelectList(countries, "Value", "Text", company.CountryId);
                }
            }

            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT c.Id, c.Name, c.City, c.Street, c.Header, c.StaffCount, c.CountryId, c.Website, c.Email, c.Edrpou, cn.Id, cn.Name " +
                               "FROM Companies c " +
                               "INNER JOIN Countries cn ON c.CountryId = cn.Id " +
                               "WHERE c.Id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        int companyId = reader.GetInt32(0);
                        string companyName = reader.GetString(1);
                        string city = reader.GetString(2);
                        string street = reader.GetString(3);
                        string header = reader.GetString(4);
                        int staffCount = reader.GetInt32(5);
                        int countryId = reader.GetInt32(6);
                        string website = reader.GetString(7);
                        string email = reader.GetString(8);
                        int edrpou = reader.GetInt32(9);
                        int countryId1 = reader.GetInt32(10);
                        string countryName = reader.GetString(11);

                        Company company = new Company
                        {
                            Id = companyId,
                            Name = companyName,
                            City = city,
                            Street = street,
                            Header = header,
                            StaffCount = staffCount,
                            CountryId = countryId1,
                            Website = website,
                            Email = email,
                            Edrpou = edrpou,
                            Country = new Country
                            {
                                Id = countryId,
                                Name = countryName
                            }
                        };

                        ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", company.CountryId);
                        return View(company);
                    }
                }
            }

            return NotFound();
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string deleteQuery = "DELETE FROM Companies WHERE Id = @Id";

                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return NotFound();
        }

        private bool CompanyExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string existsQuery = "SELECT COUNT(*) FROM Companies WHERE Id = @Id";

                using (SqlCommand existsCommand = new SqlCommand(existsQuery, connection))
                {
                    existsCommand.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    int count = (int)existsCommand.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}

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
    public class CitizensController : Controller
    {
        private readonly ItcompanyDbContext _context;
        public const string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=ITCompanyDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public CitizensController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Citizens
        public async Task<IActionResult> Index()
        {
            List<Citizen> citizens = new List<Citizen>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Citizens c INNER JOIN Countries co ON c.CountryId = co.Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Citizen citizen = new Citizen();
                        citizen.Id = (int)reader["Id"];
                        citizen.Passport = (int)reader["Passport"];
                        citizen.IdentifyCode = (int)reader["IdentifyCode"];
                        citizen.FullName = reader["FullName"].ToString();
                        citizen.Age = (int)reader["Age"];
                        citizen.City = reader["City"].ToString();
                        citizen.Street = reader["Street"].ToString();
                        citizen.NoHouse = (int)reader["NoHouse"];
                        citizen.NoFlat = (int)reader["NoFlat"];
                        citizen.Education = reader["Education"].ToString();
                        citizen.CountryId = (int)reader["CountryId"];
                        citizen.Country = new Country
                        {
                            Id = (int)reader["CountryId"],
                            Name = reader["Name"].ToString(),
                            Continent = reader["Continent"].ToString(),
                            Type = reader["Type"].ToString(),
                            Gdp = (int)reader["GDP"],
                            Header = reader["Header"].ToString(),
                            Capital = reader["Capital"].ToString(),
                            Population = (int)reader["Population"],
                            Currency = reader["Currency"].ToString(),
                            Area = (int)reader["Area"]
                        }; 

                        citizens.Add(citizen);
                    }

                    reader.Close();
                }
            }

            return View(citizens);
        }
/*
        // GET: Citizens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Citizens == null)
            {
                return NotFound();
            }

            var citizen = await _context.Citizens
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (citizen == null)
            {
                return NotFound();
            }

            return View(citizen);
        }
*/
        // GET: Citizens/Create
        public IActionResult Create()
        {
            List<Country> countries = new List<Country>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Countries";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int countryId = (int)reader["Id"];
                        string countryName = reader["Name"].ToString();

                        countries.Add(new Country
                        {
                            Id = countryId,
                            Name = countryName
                        });
                    }

                    reader.Close();
                }
            }
            ViewData["CountryId"] = new SelectList(countries, "Id", "Name");
            return View();
        }

        // POST: Citizens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Passport,IdentifyCode,FullName,Age,City,Street,NoHouse,NoFlat,Education,CountryId")] Citizen citizen)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Citizens (Passport, IdentifyCode, FullName, Age, City, Street, NoHouse, NoFlat, Education, CountryId) " +
                                   "VALUES (@Passport, @IdentifyCode, @FullName, @Age, @City, @Street, @NoHouse, @NoFlat, @Education, @CountryId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Passport", citizen.Passport);
                        command.Parameters.AddWithValue("@IdentifyCode", citizen.IdentifyCode);
                        command.Parameters.AddWithValue("@FullName", citizen.FullName);
                        command.Parameters.AddWithValue("@Age", citizen.Age);
                        command.Parameters.AddWithValue("@City", citizen.City);
                        command.Parameters.AddWithValue("@Street", citizen.Street);
                        command.Parameters.AddWithValue("@NoHouse", citizen.NoHouse);
                        command.Parameters.AddWithValue("@NoFlat", citizen.NoFlat);
                        command.Parameters.AddWithValue("@Education", citizen.Education);
                        command.Parameters.AddWithValue("@CountryId", citizen.CountryId);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(citizen);
        }

        // GET: Citizens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Citizens == null)
            {
                return NotFound();
            }

            var citizen = await _context.Citizens.FindAsync(id);
            if (citizen == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View(citizen);
        }

        // POST: Citizens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Passport,IdentifyCode,FullName,Age,City,Street,NoHouse,NoFlat,Education,CountryId")] Citizen citizen)
        {
            if (id != citizen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Citizens SET Passport = @Passport, IdentifyCode = @IdentifyCode, FullName = @FullName, Age = @Age, " +
                                   "City = @City, Street = @Street, NoHouse = @NoHouse, NoFlat = @NoFlat, Education = @Education, CountryId = @CountryId " +
                                   "WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Passport", citizen.Passport);
                        command.Parameters.AddWithValue("@IdentifyCode", citizen.IdentifyCode);
                        command.Parameters.AddWithValue("@FullName", citizen.FullName);
                        command.Parameters.AddWithValue("@Age", citizen.Age);
                        command.Parameters.AddWithValue("@City", citizen.City);
                        command.Parameters.AddWithValue("@Street", citizen.Street);
                        command.Parameters.AddWithValue("@NoHouse", citizen.NoHouse);
                        command.Parameters.AddWithValue("@NoFlat", citizen.NoFlat);
                        command.Parameters.AddWithValue("@Education", citizen.Education);
                        command.Parameters.AddWithValue("@CountryId", citizen.CountryId);
                        command.Parameters.AddWithValue("@Id", citizen.Id);

                        try
                        {
                            await connection.OpenAsync();
                            int rowsAffected = await command.ExecuteNonQueryAsync();

                            if (rowsAffected == 0)
                            {
                                return NotFound();
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["CountryId"] = GetCountrySelectList(citizen.CountryId);
            return View(citizen);
        }

        private SelectList GetCountrySelectList(int selectedCountryId)
        {
            List<Country> countries = new List<Country>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Countries";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int countryId = (int)reader["Id"];
                        string countryName = reader["Name"].ToString();

                        countries.Add(new Country
                        {
                            Id = countryId,
                            Name = countryName
                        });
                    }

                    reader.Close();
                }
            }

            return new SelectList(countries, "Id", "Name", selectedCountryId);
        }

        // GET: Citizens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Citizens WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                await reader.ReadAsync();

                                int citizenId = reader.GetInt32(reader.GetOrdinal("Id"));
                                int passport = reader.GetInt32(reader.GetOrdinal("Passport"));
                                int identifyCode = reader.GetInt32(reader.GetOrdinal("IdentifyCode"));
                                string fullName = reader.GetString(reader.GetOrdinal("FullName"));
                                int age = reader.GetInt32(reader.GetOrdinal("Age"));
                                string city = reader.GetString(reader.GetOrdinal("City"));
                                string street = reader.GetString(reader.GetOrdinal("Street"));
                                int noHouse = reader.GetInt32(reader.GetOrdinal("NoHouse"));
                                int noFlat = reader.GetInt32(reader.GetOrdinal("NoFlat"));
                                string education = reader.GetString(reader.GetOrdinal("Education"));
                                int countryId = reader.GetInt32(reader.GetOrdinal("CountryId"));

                                Citizen citizen = new Citizen
                                {
                                    Id = citizenId,
                                    Passport = passport,
                                    IdentifyCode = identifyCode,
                                    FullName = fullName,
                                    Age = age,
                                    City = city,
                                    Street = street,
                                    NoHouse = noHouse,
                                    NoFlat = noFlat,
                                    Education = education,
                                    CountryId = countryId
                                };

                                return View(citizen);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        // POST: Citizens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Citizens WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        private bool CitizenExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Citizens WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();

                        return count > 0;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}

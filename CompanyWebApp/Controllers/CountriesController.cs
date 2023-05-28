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
    public class CountriesController : Controller
    {
        private readonly ItcompanyDbContext _context;

        public const string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=ITCompanyDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public CountriesController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            List<Country> countries = new List<Country>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Countries";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Country country = new Country
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Continent = (string)reader["Continent"],
                                Type = (string)reader["Type"],
                                Gdp = (int)reader["Gdp"],
                                Header = (string)reader["Header"],
                                Capital = (string)reader["Capital"],
                                Population = (int)reader["Population"],
                                Currency = (string)reader["Currency"],
                                Area = (int)reader["Area"]
                            };

                            countries.Add(country);
                        }
                    }
                }
            }

            return View(countries);
        }
        /*
        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }
        */
        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Continent,Type,Gdp,Header,Capital,Population,Currency,Area")] Country country)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Countries (Name, Continent, Type, Gdp, Header, Capital, Population, Currency, Area) " +
                                   "VALUES (@Name, @Continent, @Type, @Gdp, @Header, @Capital, @Population, @Currency, @Area)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", country.Name);
                        command.Parameters.AddWithValue("@Continent", country.Continent);
                        command.Parameters.AddWithValue("@Type", country.Type);
                        command.Parameters.AddWithValue("@Gdp", country.Gdp);
                        command.Parameters.AddWithValue("@Header", country.Header);
                        command.Parameters.AddWithValue("@Capital", country.Capital);
                        command.Parameters.AddWithValue("@Population", country.Population);
                        command.Parameters.AddWithValue("@Currency", country.Currency);
                        command.Parameters.AddWithValue("@Area", country.Area);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Countries WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            Country country = new Country
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Continent = reader["Continent"].ToString(),
                                Type = reader["Type"].ToString(),
                                Gdp = Convert.ToInt32(reader["Gdp"]),
                                Header = reader["Header"].ToString(),
                                Capital = reader["Capital"].ToString(),
                                Population = Convert.ToInt32(reader["Population"]),
                                Currency = reader["Currency"].ToString(),
                                Area = Convert.ToInt32(reader["Area"])
                            };

                            return View(country);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Continent,Type,Gdp,Header,Capital,Population,Currency,Area")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Countries SET Name = @Name, Continent = @Continent, Type = @Type, Gdp = @Gdp, Header = @Header, Capital = @Capital, Population = @Population, Currency = @Currency, Area = @Area WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", country.Name);
                        command.Parameters.AddWithValue("@Continent", country.Continent);
                        command.Parameters.AddWithValue("@Type", country.Type);
                        command.Parameters.AddWithValue("@Gdp", country.Gdp);
                        command.Parameters.AddWithValue("@Header", country.Header);
                        command.Parameters.AddWithValue("@Capital", country.Capital);
                        command.Parameters.AddWithValue("@Population", country.Population);
                        command.Parameters.AddWithValue("@Currency", country.Currency);
                        command.Parameters.AddWithValue("@Area", country.Area);
                        command.Parameters.AddWithValue("@Id", country.Id);

                        connection.Open();
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
                }
            }

            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Countries WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            Country country = new Country
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Continent = (string)reader["Continent"],
                                Type = (string)reader["Type"],
                                Gdp = (int)reader["Gdp"],
                                Header = (string)reader["Header"],
                                Capital = (string)reader["Capital"],
                                Population = (int)reader["Population"],
                                Currency = (string)reader["Currency"],
                                Area = (int)reader["Area"]
                            };

                            return View(country);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Countries WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
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
            }
        }

        private bool CountryExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Countries WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}

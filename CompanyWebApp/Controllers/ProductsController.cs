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
    public class ProductsController : Controller
    {
        private readonly ItcompanyDbContext _context;

        public const string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=ITCompanyDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        public ProductsController(ItcompanyDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.Id, p.Name, p.Version, p.Appointment, p.Distribution, p.ReleaseDate, p.Price, p.Language, p.Features, p.Info, p.CompanyId, c.Name AS CompanyName FROM Products p INNER JOIN Companies c ON p.CompanyId = c.Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Product> products = new List<Product>();

                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Version = (string)reader["Version"],
                                Appointment = (string)reader["Appointment"],
                                Distribution = (string)reader["Distribution"],
                                ReleaseDate = (string)reader["ReleaseDate"],
                                Price = (int)reader["Price"],
                                Language = (string)reader["Language"],
                                Features = (string)reader["Features"],
                                Info = (string)reader["Info"],
                                CompanyId = (int)reader["CompanyId"],
                                Company = new Company
                                {
                                    Id = (int)reader["CompanyId"],
                                    Name = (string)reader["CompanyName"]
                                }
                            };

                            products.Add(product);
                        }

                        return View(products);
                    }
                }
            }

        }
        /*
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        */
        // GET: Products/Create
        public IActionResult Create()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Companies";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<SelectListItem> companies = new List<SelectListItem>();

                        while (reader.Read())
                        {
                            int companyId = (int)reader["Id"];
                            string companyName = (string)reader["Name"];

                            companies.Add(new SelectListItem { Value = companyId.ToString(), Text = companyName });
                        }

                        ViewData["CompanyId"] = companies;
                    }
                }
            }

            return View();

        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Version,Appointment,Distribution,ReleaseDate,Price,Language,Features,Info,CompanyId")] Product product)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string insertQuery = "INSERT INTO Products (Name, Version, Appointment, Distribution, ReleaseDate, Price, Language, Features, Info, CompanyId) " +
                                         "VALUES (@Name, @Version, @Appointment, @Distribution, @ReleaseDate, @Price, @Language, @Features, @Info, @CompanyId)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Version", product.Version);
                        command.Parameters.AddWithValue("@Appointment", product.Appointment);
                        command.Parameters.AddWithValue("@Distribution", product.Distribution);
                        command.Parameters.AddWithValue("@ReleaseDate", product.ReleaseDate);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Language", product.Language);
                        command.Parameters.AddWithValue("@Features", product.Features);
                        command.Parameters.AddWithValue("@Info", product.Info);
                        command.Parameters.AddWithValue("@CompanyId", product.CompanyId);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Name FROM Companies";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<SelectListItem> companies = new List<SelectListItem>();

                        while (reader.Read())
                        {
                            int companyId = (int)reader["Id"];
                            string companyName = (string)reader["Name"];
                            var Company = new Company { Id = companyId, Name = companyName };
                            companies.Add(new SelectListItem { Value = companyId.ToString(), Text = companyName });
                        }

                        ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", product.CompanyId);
                    }
                }
            }

            return View(product);

        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT p.Id, p.Name, p.Version, p.Appointment, p.Distribution, p.ReleaseDate, p.Price, p.Language, p.Features, p.Info, p.CompanyId, c.Name AS CompanyName " +
                               "FROM Products AS p " +
                               "INNER JOIN Companies AS c ON p.CompanyId = c.Id " +
                               "WHERE p.Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int productId = (int)reader["Id"];
                            string productName = (string)reader["Name"];
                            string productVersion = (string)reader["Version"];
                            string productAppointment = (string)reader["Appointment"];
                            string productDistribution = (string)reader["Distribution"];
                            string productReleaseDate = (string)reader["ReleaseDate"];
                            int productPrice = (int)reader["Price"];
                            string productLanguage = (string)reader["Language"];
                            string productFeatures = (string)reader["Features"];
                            string productInfo = (string)reader["Info"];
                            int companyId = (int)reader["CompanyId"];
                            string companyName = (string)reader["CompanyName"];

                            var product = new Product
                            {
                                Id = productId,
                                Name = productName,
                                Version = productVersion,
                                Appointment = productAppointment,
                                Distribution = productDistribution,
                                ReleaseDate = productReleaseDate,
                                Price = productPrice,
                                Language = productLanguage,
                                Features = productFeatures,
                                Info = productInfo,
                                CompanyId = companyId,
                                Company = new Company { Id = companyId, Name = companyName }
                            };

                            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", product.CompanyId);
                            return View(product);
                        }
                    }
                }
            }

            return NotFound();

        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Version,Appointment,Distribution,ReleaseDate,Price,Language,Features,Info,CompanyId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE Products " +
                                     "SET Name = @Name, Version = @Version, Appointment = @Appointment, Distribution = @Distribution, " +
                                     "ReleaseDate = @ReleaseDate, Price = @Price, Language = @Language, Features = @Features, " +
                                     "Info = @Info, CompanyId = @CompanyId " +
                                     "WHERE Id = @Id";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", product.Name);
                    updateCommand.Parameters.AddWithValue("@Version", product.Version);
                    updateCommand.Parameters.AddWithValue("@Appointment", product.Appointment);
                    updateCommand.Parameters.AddWithValue("@Distribution", product.Distribution);
                    updateCommand.Parameters.AddWithValue("@ReleaseDate", product.ReleaseDate);
                    updateCommand.Parameters.AddWithValue("@Price", product.Price);
                    updateCommand.Parameters.AddWithValue("@Language", product.Language);
                    updateCommand.Parameters.AddWithValue("@Features", product.Features);
                    updateCommand.Parameters.AddWithValue("@Info", product.Info);
                    updateCommand.Parameters.AddWithValue("@CompanyId", product.CompanyId);
                    updateCommand.Parameters.AddWithValue("@Id", product.Id);

                    connection.Open();
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            Product product;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT p.Id, p.Name, p.Version, p.Appointment, p.Distribution, p.ReleaseDate, " +
                                     "p.Price, p.Language, p.Features, p.Info, p.CompanyId, c.Id, c.Name AS CompanyName " +
                                     "FROM Products p " +
                                     "INNER JOIN Companies c ON p.CompanyId = c.Id " +
                                     "WHERE p.Id = @Id";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    SqlDataReader reader = selectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        product = new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Version = reader["Version"].ToString(),
                            Appointment = reader["Appointment"].ToString(),
                            Distribution = reader["Distribution"].ToString(),
                            ReleaseDate = reader["ReleaseDate"].ToString(),
                            Price = Convert.ToInt32(reader["Price"]),
                            Language = reader["Language"].ToString(),
                            Features = reader["Features"].ToString(),
                            Info = reader["Info"].ToString(),
                            CompanyId = Convert.ToInt32(reader["CompanyId"]),
                            Company = new Company
                            {
                                Id = (int)reader["CompanyId"],
                                Name = (string)reader["CompanyName"]
                            }
                        };
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string deleteQuery = "DELETE FROM Products WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM Products WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }

        }
    }
}

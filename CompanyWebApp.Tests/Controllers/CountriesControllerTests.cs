using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using CompanyWebApp.Controllers;
using CompanyWebApp.Models;
using System.Data.SqlClient;

namespace CompanyWebApp.Tests
{
    [TestFixture]
    [Category("UnitTests")]
    public class CountriesControllerTests
    {
        private CountriesController _controller;
        private ItcompanyDbContext _context;
        private DbContextOptions<ItcompanyDbContext> _options;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _options = new DbContextOptionsBuilder<ItcompanyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ItcompanyDbContext(_options);


            _context.Countries.Add(new Country
            {
                Id = 1,
                Name = "Test Country",
                Continent = "Test Continent",
                Type = "Test Type",
                Gdp = 100,
                Header = "Test Header",
                Capital = "Test Capital",
                Population = 10,
                Currency = "Test Currency",
                Area = 1000
            });
            
            _context.Countries.Add(new Country
                        {
                            Id = 2,
                            Name = "Test Country",
                            Continent = "Test Continent",
                            Type = "Test Type",
                            Gdp = 100,
                            Header = "Test Header",
                            Capital = "Test Capital",
                            Population = 10,
                            Currency = "Test Currency",
                            Area = 1000
                        });

            _context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new CountriesController(_context);
        }

        [Test, Category("Index")]
        public async Task Index_Returns_View_With_Countries()
        {
            var result = await _controller.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as List<Country>;
            Assert.IsNotNull(model);
            Assert.IsNotEmpty(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test, Category("Create")]
        public async Task Create_Returns_View()
        {
            var result = _controller.Create();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test, Category("Create")]
        public async Task Create_Post_Valid_Country()
        {
            var newCountry = new Country
            {
                Name = "New Country",
                Continent = "New Continent",
                Type = "New Type",
                Gdp = 150,
                Header = "New Header",
                Capital = "New Capital",
                Population = 20,
                Currency = "New Currency",
                Area = 2000
            };

            var result = await _controller.Create(newCountry);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var countries = await _context.Countries.ToListAsync();
            Assert.AreEqual(3, countries.Count);
        }

        [Test, Category("Create")]
        public async Task Create_Post_Invalid_Country()
        {
            var newCountry = new Country
            {
                Name = "", // Invalid name
                Continent = "New Continent",
                Type = "New Type",
                Gdp = 150,
                Header = "New Header",
                Capital = "New Capital",
                Population = 20,
                Currency = "New Currency",
                Area = 2000
            };

            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(newCountry);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Country;
            Assert.IsNotNull(model);
            Assert.AreEqual("", model.Name);
        }

        [Test, Category("Edit")]
        public async Task Edit_Returns_View_For_Valid_Id()
        {
            var result = await _controller.Edit(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Country;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test, Category("Edit")]
        public async Task Edit_Returns_NotFound_For_Invalid_Id()
        {
            var result = await _controller.Edit(999);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test, Category("Edit")]
        public async Task Edit_Post_Valid_Country()
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == 1);
            country.Name = "Updated Country";

            var result = await _controller.Edit(1, country);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var updatedCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == 1);
            Assert.AreEqual("Updated Country", updatedCountry.Name);
        }

        [Test, Category("Edit")]
        public async Task Edit_Post_Invalid_Country()
        {
            var country = new Country
            {
                Id = 1,
                Name = "", // Invalid name
                Continent = "New Continent",
                Type = "New Type",
                Gdp = 150,
                Header = "New Header",
                Capital = "New Capital",
                Population = 20,
                Currency = "New Currency",
                Area = 2000
            };

            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Edit(1, country);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Country;
            Assert.IsNotNull(model);
        }

        [Test, Category("Delete")]
        public async Task Delete_Returns_View_For_Valid_Id()
        {
            var result = await _controller.Delete(2);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Country;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Id);
        }

        [Test, Category("Delete")]
        public async Task Delete_Returns_NotFound_For_Invalid_Id()
        {
            var result = await _controller.Delete(999);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test, Category("Delete")]
        public async Task DeleteConfirmed_Deletes_Country()
        {
            var result = await _controller.DeleteConfirmed(2);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == 2);
            Assert.IsNull(country);
        }

        

        [Test, Category("Parameterized")]
        [TestCase(1)]
        [TestCase(999)]
        public async Task CountryExists_TestCases(int id)
        {
            var exists = _controller.CountryExists(id);
            if (id == 1)
            {
                Assert.IsTrue(exists);
            }
            else
            {
                Assert.IsFalse(exists);
            }
        }
    }
}

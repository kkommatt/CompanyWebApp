using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using CompanyWebApp.Controllers;
using CompanyWebApp.Models;

namespace CompanyWebApp.Tests
{
    [TestFixture]
    [Category("UnitTests")]
    public class CitizensControllerTests
    {
        private CitizensController _controller;
        private ItcompanyDbContext _context;
        private DbContextOptions<ItcompanyDbContext> _options;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _options = new DbContextOptionsBuilder<ItcompanyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ItcompanyDbContext(_options);

            // Seed the database with initial data
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

            _context.Citizens.AddRange(new List<Citizen>
            {
                new Citizen
                {
                    Id = 1,
                    Passport = 123456,
                    IdentifyCode = 654321,
                    FullName = "John Doe",
                    Age = 30,
                    City = "Test City",
                    Street = "Test Street",
                    NoHouse = 1,
                    NoFlat = 2,
                    Education = "Test Education",
                    CountryId = 1
                },
                new Citizen
                {
                    Id = 2,
                    Passport = 223456,
                    IdentifyCode = 754321,
                    FullName = "Jane Doe",
                    Age = 25,
                    City = "Test City 2",
                    Street = "Test Street 2",
                    NoHouse = 2,
                    NoFlat = 3,
                    Education = "Test Education 2",
                    CountryId = 1
                },
                new Citizen
                {
                    Id = 3,
                    Passport = 323456,
                    IdentifyCode = 854321,
                    FullName = "Jim Beam",
                    Age = 35,
                    City = "Test City 3",
                    Street = "Test Street 3",
                    NoHouse = 3,
                    NoFlat = 4,
                    Education = "Test Education 3",
                    CountryId = 1
                }
            });

            _context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new CitizensController(_context);
        }

        [Test]
        public async Task Index_Returns_View_With_Citizens()
        {
            var result = await _controller.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as List<Citizen>;
            Assert.IsNotNull(model);
            Assert.IsNotEmpty(model);
        }

        [Test]
        public void Create_Returns_View()
        {
            var result = _controller.Create();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_Valid_Citizen()
        {
            var newCitizen = new Citizen
            {
                Passport = 987654,
                IdentifyCode = 456789,
                FullName = "Jake Doe",
                Age = 28,
                City = "Another City",
                Street = "Another Street",
                NoHouse = 4,
                NoFlat = 5,
                Education = "Another Education",
                CountryId = 1
            };

            var result = await _controller.Create(newCitizen);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var citizens = await _context.Citizens.ToListAsync();
            Assert.AreEqual(4, citizens.Count);
        }

        [Test]
        public void Create_Post_Invalid_Citizen()
        {
            var newCitizen = new Citizen
            {
                Passport = 0, // Invalid passport
                IdentifyCode = 456789,
                FullName = "Jake Doe",
                Age = 28,
                City = "Another City",
                Street = "Another Street",
                NoHouse = 4,
                NoFlat = 5,
                Education = "Another Education",
                CountryId = 1
            };

            _controller.ModelState.AddModelError("Passport", "Required");

            var result = _controller.Create(newCitizen).Result;
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Citizen;
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task Edit_Returns_View_For_Valid_Id()
        {
            var result = await _controller.Edit(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Citizen;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test]
        public void Edit_Returns_NotFound_For_Invalid_Id()
        {
            var result = _controller.Edit(null).Result;
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_Post_Valid_Citizen()
        {
            var citizen = await _context.Citizens.FirstOrDefaultAsync(c => c.Id == 1);
            citizen.FullName = "John Smith";

            var result = await _controller.Edit(1, citizen);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var updatedCitizen = await _context.Citizens.FirstOrDefaultAsync(c => c.Id == 1);
            Assert.AreEqual("John Smith", updatedCitizen.FullName);
        }

        [Test]
        public void Edit_Post_Invalid_Citizen()
        {
            var citizen = new Citizen
            {
                Id = 1,
                Passport = 0, // Invalid passport
                IdentifyCode = 456789,
                FullName = "Jake Doe",
                Age = 28,
                City = "Another City",
                Street = "Another Street",
                NoHouse = 4,
                NoFlat = 5,
                Education = "Another Education",
                CountryId = 1
            };

            _controller.ModelState.AddModelError("Passport", "Required");

            var result = _controller.Edit(1, citizen).Result;
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Citizen;
            Assert.IsNotNull(model);
        }

        [Test]
        public void Delete_Returns_NotFound_For_Invalid_Id()
        {
            var result = _controller.Delete(null).Result;
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_Returns_View_For_Valid_Id()
        {
            var result = await _controller.Delete(3);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Citizen;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Id);
        }

        [Test]
        public async Task DeleteConfirmed_Deletes_Citizen()
        {
            var result = await _controller.DeleteConfirmed(3);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var citizen = await _context.Citizens.FirstOrDefaultAsync(c => c.Id == 3);
            Assert.IsNull(citizen);
        }

        [Test]
        public async Task CitizenExists_Returns_True_For_Valid_Id()
        {
            var exists = await _context.Citizens.AnyAsync(c => c.Id == 1);
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task CitizenExists_Returns_False_For_Invalid_Id()
        {
            var exists = await _context.Citizens.AnyAsync(c => c.Id == 999);
            Assert.IsFalse(exists);
        }

        [Test]
        [TestCase(1)]
        [TestCase(999)]
        public async Task CitizenExists_TestCases(int id)
        {
            var exists = await _context.Citizens.AnyAsync(c => c.Id == id);
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

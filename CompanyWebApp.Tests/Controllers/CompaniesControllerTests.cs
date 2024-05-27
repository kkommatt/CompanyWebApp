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
    public class CompaniesControllerTests
    {
        private CompaniesController _controller;
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

            _context.Companies.AddRange(new List<Company>
            {
                new Company
                {
                    Id = 1,
                    Name = "Test Company 1",
                    City = "Test City 1",
                    Street = "Test Street 1",
                    Header = "Test Header 1",
                    StaffCount = 100,
                    CountryId = 1,
                    Website = "http://testcompany1.com",
                    Email = "info@testcompany1.com",
                    Edrpou = 12345678
                },
                new Company
                {
                    Id = 2,
                    Name = "Test Company 2",
                    City = "Test City 2",
                    Street = "Test Street 2",
                    Header = "Test Header 2",
                    StaffCount = 200,
                    CountryId = 1,
                    Website = "http://testcompany2.com",
                    Email = "info@testcompany2.com",
                    Edrpou = 87654321
                }
            });

            _context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new CompaniesController(_context);
        }

        [Test, Category("Index")]
        public async Task Index_Returns_View_With_Companies()
        {
            var result = await _controller.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as List<Company>;
            Assert.IsNotNull(model);
            Assert.IsNotEmpty(model);
            Assert.AreEqual(2, model.Count);
        }

        [Test, Category("Create")]
        public async Task Create_Returns_View_With_Countries()
        {
            var result = await _controller.Create();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test, Category("Create")]
        public async Task Create_Post_Valid_Company()
        {
            var newCompany = new Company
            {
                Name = "New Company",
                City = "New City",
                Street = "New Street",
                Header = "New Header",
                StaffCount = 150,
                CountryId = 1,
                Website = "http://newcompany.com",
                Email = "info@newcompany.com",
                Edrpou = 56781234
            };

            var result = await _controller.Create(newCompany);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var companies = await _context.Companies.ToListAsync();
            Assert.AreEqual(3, companies.Count);
        }

        [Test, Category("Create")]
        public void Create_Post_Invalid_Company()
        {
            var newCompany = new Company
            {
                Name = "", // Invalid name
                City = "New City",
                Street = "New Street",
                Header = "New Header",
                StaffCount = 150,
                CountryId = 1,
                Website = "http://newcompany.com",
                Email = "info@newcompany.com",
                Edrpou = 56781234
            };

            _controller.ModelState.AddModelError("Name", "Required");

            var result = _controller.Create(newCompany).Result;
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Company;
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

            var model = viewResult.Model as Company;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test, Category("Edit")]
        public void Edit_Returns_NotFound_For_Invalid_Id()
        {
            var result = _controller.Edit(null).Result;
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test, Category("Edit")]
        public async Task Edit_Post_Valid_Company()
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == 1);
            company.Name = "Updated Company";

            var result = await _controller.Edit(1, company);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var updatedCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == 1);
            Assert.AreEqual("Updated Company", updatedCompany.Name);
        }

        [Test, Category("Edit")]
        public void Edit_Post_Invalid_Company()
        {
            var company = new Company
            {
                Id = 1,
                Name = "", // Invalid name
                City = "New City",
                Street = "New Street",
                Header = "New Header",
                StaffCount = 150,
                CountryId = 1,
                Website = "http://newcompany.com",
                Email = "info@newcompany.com",
                Edrpou = 56781234
            };

            _controller.ModelState.AddModelError("Name", "Required");

            var result = _controller.Edit(1, company).Result;
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Company;
            Assert.IsNotNull(model);
        }

        [Test, Category("Delete")]
        public void Delete_Returns_NotFound_For_Invalid_Id()
        {
            var result = _controller.Delete(null).Result;
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test, Category("Delete")]
        public async Task Delete_Returns_View_For_Valid_Id()
        {
            var result = await _controller.Delete(2);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Company;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Id);
        }

        [Test, Category("Delete")]
        public async Task DeleteConfirmed_Deletes_Company()
        {
            var result = await _controller.DeleteConfirmed(2);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == 2);
            Assert.IsNull(company);
        }

        

        [Test, Category("Parameterized")]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(999)]
        public async Task CompanyExists_TestCases(int id)
        {
            var exists = await _context.Companies.AnyAsync(c => c.Id == id);
            if (id == 1 || id == 2)
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

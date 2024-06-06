using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using CompanyWebApp.Controllers;
using CompanyWebApp.Models;

namespace CompanyWebApp.Tests
{
    [TestFixture]
    [Category("ProductsControllerTests")]
    public class ProductsControllerTests
    {
        private ProductsController _controller;
        private ItcompanyDbContext _context;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<ItcompanyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ItcompanyDbContext(options);

            // Seed the in-memory database
            _context.Companies.AddRange(
                new Company
                {
                    Id = 10,
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
                    Id = 20,
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
            );

            _context.Products.AddRange(
                new Product { Id = 1, Name = "Product1", Version = "1.0", Appointment = "Test", Distribution = "Online", ReleaseDate = "2024-01-01", Price = 100, Language = "C#", Features = "Feature1", Info = "Info1", CompanyId = 10 },
                new Product { Id = 2, Name = "Product2", Version = "2.0", Appointment = "Test", Distribution = "Offline", ReleaseDate = "2024-02-01", Price = 200, Language = "Java", Features = "Feature2", Info = "Info2", CompanyId = 20 },
                new Product { Id = 4, Name = "Product2", Version = "2.0", Appointment = "Test", Distribution = "Offline", ReleaseDate = "2024-02-01", Price = 200, Language = "Java", Features = "Feature2", Info = "Info2", CompanyId = 20 }
                );

            _context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new ProductsController(_context);
        }

        [Test, Category("Index")]
        public async Task Index_Returns_View_With_Products()
        {
            var result = await _controller.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as List<Product>;
            Assert.IsNotNull(model);
            Assert.IsNotEmpty(model);
        }

        [Test, Category("Create")]
        public void Create_Returns_View()
        {
            var result = _controller.Create();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test, Category("Create")]
        public async Task Create_Adds_Product_And_Redirects()
        {
            var newProduct = new Product { Id = 3, Name = "Product3", Version = "3.0", Appointment = "Test", Distribution = "Online", ReleaseDate = "2024-03-01", Price = 300, Language = "Python", Features = "Feature3", Info = "Info3", CompanyId = 10 };

            var result = await _controller.Create(newProduct);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);

            var product = await _context.Products.FindAsync(3);
            Assert.IsNotNull(product);
        }

        [Test, Category("Edit")]
        public async Task Edit_Returns_View_With_Product()
        {
            var result = await _controller.Edit(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as Product;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [Test, Category("Edit")]
        public void Edit_Throws_Exception_For_Invalid_Id()
        {
            var ex = Assert.ThrowsAsync<Exception>(async () => await _controller.Edit(99));
            Assert.IsNotNull(ex);
        }

        [Test, Category("Parameterized")]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Edit_Returns_Correct_Product(int productId)
        {
            var result = await _controller.Edit(productId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as Product;
            Assert.IsNotNull(model);
            Assert.AreEqual(productId, model.Id);
        }

        [Test, Category("Delete")]
        public async Task Delete_Returns_View_With_Product()
        {
            var result = await _controller.Delete(4);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as Product;
            Assert.IsNotNull(model);
            Assert.AreEqual(4, model.Id);
        }

        [Test, Category("Delete")]
        public async Task DeleteConfirmed_Deletes_Product()
        {
            var result = await _controller.DeleteConfirmed(4);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);

            var product = await _context.Products.FindAsync(4);
            Assert.IsNull(product);
        }
    }
}

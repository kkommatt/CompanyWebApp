using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using CompanyWebApp.Models;

namespace CompanyWebApp.Tests
{
    [TestFixture]
    public class DbContextTests
    {
        private DbContextOptions<ItcompanyDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ItcompanyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ItcompanyDbContext(_options))
            {
                // Create a new Country entity
                var newCountry = new Country
                {
                    Name = "Test Country",
                    Continent = "Test Continent",
                    Type = "Test Type",
                    Gdp = 100, // Example value for Gdp
                    Header = "Test Header",
                    Capital = "Test Capital",
                    Population = 10, // Example value for Population
                    Currency = "Test Currency",
                    Area = 1000 // Example value for Area
                };

                // Add the new country to the DbSet
                context.Countries.Add(newCountry);

                // Save changes to the database
                context.SaveChanges();
            }
        }

        [Test]
        public void CanRetrieveData()
        {
            using (var context = new ItcompanyDbContext(_options))
            {
                // Perform test queries or actions here
                var countries = context.Countries.ToList();

                Assert.IsNotNull(countries);
                
            }
        }

    }
}
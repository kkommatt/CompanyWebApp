using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using CompanyWebApp.Controllers;
using CompanyWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace CompanyWebApp.Tests
{
    [TestFixture]
    [Category("HomeControllerTests")]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private ILogger<HomeController> _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // This runs once before all tests
        }

        [SetUp]
        public void Setup()
        {
            // Using a simple logger for testing purposes
            _logger = new SimpleLogger<HomeController>();
            _controller = new HomeController(_logger);
        }

        [Test, Category("Index")]
        public void Index_Returns_View()
        {
            var result = _controller.Index();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test, Category("Privacy")]
        public void Privacy_Returns_View()
        {
            var result = _controller.Privacy();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }


        [Test, Category("Exceptions")]
        public void Error_Returns_ErrorViewModel_Without_RequestId()
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext = controllerContext;

            var result = _controller.Error();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.IsFalse(string.IsNullOrEmpty(model.RequestId));
        }

        [Test, Category("Parameterized")]
        [TestCase("TestId1")]
        [TestCase("TestId2")]
        public void Error_Returns_ErrorViewModel_With_Custom_RequestId(string requestId)
        {
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controllerContext.HttpContext.TraceIdentifier = requestId;
            _controller.ControllerContext = controllerContext;

            var result = _controller.Error();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as ErrorViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(requestId, model.RequestId);
        }
    }

    public class SimpleLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            // Simple logger implementation for testing purposes
            Console.WriteLine($"{logLevel.ToString()}: {formatter(state, exception)}");
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dapperOdata.Controllers;
using dapperOdata.Models;
using System;
using System.Web.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;


namespace dapperOdata.Tests
{
    [TestClass]
    public class BooksControllerTest
    {

        private BooksController _controller;

        [TestInitialize]
        public void Setup() {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            _controller = new BooksController(new Repo(config));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void TestGet(int id)
        {

            var result = _controller.Get(id) as OkObjectResult;
            var book = result.Value as BookFast;
            Assert.AreEqual(book.Id, id);
        }

        [TestMethod]
        public void TestGetNotFound()
        {
            var result = _controller.Get(100);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void TestGetList()
        {

            var result = _controller.Get() as OkObjectResult;
            var values = result.Value as List<BookFast>;

            Assert.IsTrue(values.Count > 1);
        }

        [TestMethod]
        public void TestPost()
        {

            var b = new BookFast{Title="Inline-title"};
            var result = _controller.Post(b) as ObjectResult;
            var book = result.Value as BookFast;

            Assert.AreEqual(book.Title, b.Title);
            Assert.IsNotNull(book.Id);
        }

        [TestMethod]
        public void TestPostFailed()
        {

            var b = new BookFast{ISBN="missing title"};
            _controller.ModelState.AddModelError("Title", "Empty"); //validate with lifecycle
            var result = _controller.Post(b);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DisasterReportsIntegrationTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private IServiceScope _scope;
        private ApplicationDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _client = _factory.CreateClient();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task IndexPage_ShouldDisplayDisasterReports_FromDatabase()
        {
            // --- ARRANGE ---
            var testReport1 = new DisasterReport
            {
                Type = "Flood",
                Location = "Test Location 1",
                Severity = "High",
                Description = "Massive flooding in area.",
                DateReported = DateTime.Now
            };
            var testReport2 = new DisasterReport
            {
                Type = "Fire",
                Location = "Test Location 2",
                Severity = "Medium",
                Description = "Wildfire spreading.",
                DateReported = DateTime.Now
            };

            await _context.DisasterReports.AddRangeAsync(testReport1, testReport2);
            await _context.SaveChangesAsync();

            // --- ACT ---
            var response = await _client.GetAsync("/DisasterReports"); // Changed URL
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            // --- ASSERT ---
            Assert.IsTrue(html.Contains("Test Location 1"));
            Assert.IsTrue(html.Contains("Test Location 2"));
            Assert.IsTrue(html.Contains("Flood"));
            Assert.IsTrue(html.Contains("Wildfire spreading."));
        }

        [TestCleanup]
        public void Teardown()
        {
            _client.Dispose();
            _factory.Dispose();
            _scope.Dispose();
            _context.Dispose();
        }
    }
}

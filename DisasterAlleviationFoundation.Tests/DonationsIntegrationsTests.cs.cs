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
    public class DonationsIntegrationTests
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
        public async Task IndexPage_ShouldDisplayDonations_FromDatabase()
        {
            // --- ARRANGE ---
            var testDonation1 = new Donation
            {
                DonorName = "Integration Test Donor 1",
                ResourceType = "Food",
                Description = "Canned goods",
                Quantity = 50,
                DateDonated = DateTime.Now
            };
            var testDonation2 = new Donation
            {
                DonorName = "Integration Test Donor 2",
                ResourceType = "Water",
                Description = "Bottled water",
                Quantity = 100,
                DateDonated = DateTime.Now
            };

            await _context.Donations.AddRangeAsync(testDonation1, testDonation2);
            await _context.SaveChangesAsync();

            // --- ACT ---
            var response = await _client.GetAsync("/Donations");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            // --- ASSERT ---
            Assert.IsTrue(html.Contains("Integration Test Donor 1"));
            Assert.IsTrue(html.Contains("Integration Test Donor 2"));
            Assert.IsTrue(html.Contains("Canned goods"));
            Assert.IsTrue(html.Contains("Bottled water"));
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


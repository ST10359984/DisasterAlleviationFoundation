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
using System.Text.RegularExpressions;
using System.Linq;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass]
    public class DonationsIntegrationTests
    {
        private HttpClient _client = null!;
        private WebApplicationFactory<Program> _factory = null!;
        private IServiceScope _scope = null!;
        private ApplicationDbContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task IndexPage_ShouldDisplayDonations_FromDatabase()
        {
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

            var response = await _client.GetAsync("/Donations");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(html.Contains("Integration Test Donor 1"));
            Assert.IsTrue(html.Contains("Integration Test Donor 2"));
            Assert.IsTrue(html.Contains("Canned goods"));
            Assert.IsTrue(html.Contains("Bottled water"));
        }

        [TestMethod]
        public async Task OnPostAsync_ShouldCreateDonation_WhenPostIsValid()
        {
            var response = await _client.GetAsync("/Donations/Create");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            var token = GetAntiForgeryToken(html);

            var formData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "Donation.DonorName", "Integration Post Test" },
                { "Donation.ResourceType", "Test Resource" },
                { "Donation.Description", "A test donation" },
                { "Donation.Quantity", "123" },
                { "Donation.DateDonated", DateTime.Now.ToString("yyyy-MM-dd") }
            };
            var httpContent = new FormUrlEncodedContent(formData);

            response = await _client.PostAsync("/Donations/Create", httpContent);

            Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            Assert.AreEqual("/", response.Headers.Location.OriginalString);

            var savedDonation = _context.Donations.FirstOrDefault(d => d.DonorName == "Integration Post Test");
            Assert.IsNotNull(savedDonation);
            Assert.AreEqual("A test donation", savedDonation.Description);
            Assert.AreEqual(123, savedDonation.Quantity);
        }

        [TestCleanup]
        public void Teardown()
        {
            _client.Dispose();
            _factory.Dispose();
            _scope.Dispose();
            _context.Dispose();
        }

        private string GetAntiForgeryToken(string html)
        {
            var match = Regex.Match(html, @"<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" />");

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            throw new InvalidOperationException("Could not find anti-forgery token in HTML response.");
        }
    }
}


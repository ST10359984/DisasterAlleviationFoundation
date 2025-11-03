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
    [TestCategory("Integration")]
    [TestClass]
    public class DisasterReportsIntegrationTests
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
        public async Task IndexPage_ShouldDisplayDisasterReports_FromDatabase()
        {
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

            var response = await _client.GetAsync("/DisasterReports");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.IsTrue(html.Contains("Test Location 1"));
            Assert.IsTrue(html.Contains("Test Location 2"));
            Assert.IsTrue(html.Contains("Flood"));
            Assert.IsTrue(html.Contains("Wildfire spreading."));
        }

        [TestMethod]
        public async Task OnPostAsync_ShouldCreateDisasterReport_WhenPostIsValid()
        {
            var response = await _client.GetAsync("/DisasterReports/Create");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            var token = GetAntiForgeryToken(html);

            var formData = new Dictionary<string, string>
            {
                { "__RequestVerificationToken", token },
                { "DisasterReport.Type", "Test Fire" },
                { "DisasterReport.Location", "Integration Post Location" },
                { "DisasterReport.Severity", "Critical" },
                { "DisasterReport.Description", "A test disaster report" },
                { "DisasterReport.DateReported", DateTime.Now.ToString("yyyy-MM-dd") }
            };
            var httpContent = new FormUrlEncodedContent(formData);

            response = await _client.PostAsync("/DisasterReports/Create", httpContent);

            Assert.AreEqual(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            // --- THIS IS THE FIX ---
            // The root Index page URL is "/", not "/Index".
            Assert.AreEqual("/", response.Headers.Location.OriginalString);

            var savedReport = _context.DisasterReports.FirstOrDefault(d => d.Location == "Integration Post Location");
            Assert.IsNotNull(savedReport);
            Assert.AreEqual("A test disaster report", savedReport.Description);
            Assert.AreEqual("Critical", savedReport.Severity);
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


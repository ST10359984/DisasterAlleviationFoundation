using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviationFoundation.Pages.Donations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System;

namespace DisasterAlleviationFoundation.Tests
{
    [TestClass] 
    public class DonationsCreateModelTests
    {
        private ApplicationDbContext _context;
        private CreateModel _pageModel;

        [TestInitialize] 
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _pageModel = new CreateModel(_context);
        }

        [TestMethod] 
        public async Task OnPostAsync_ShouldSaveDonation_WhenModelStateIsValid()
        {
            var newDonation = new Donation
            {
                DonorName = "Test Donor",
                ResourceType = "Water",
                Description = "100 bottles",
                Quantity = 100,
                DateDonated = DateTime.Now
            };

            _pageModel.Donation = newDonation;

            var result = await _pageModel.OnPostAsync();

            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            Assert.AreEqual("/Index", ((RedirectToPageResult)result).PageName);

            Assert.AreEqual(1, _context.Donations.Count()); 

            var savedDonation = _context.Donations.First();
            Assert.AreEqual("Test Donor", savedDonation.DonorName);
            Assert.AreEqual(100, savedDonation.Quantity);
        }

        [TestMethod] 
        public async Task OnPostAsync_ShouldReturnPage_WhenModelStateIsInvalid()
        {
            var newDonation = new Donation
            {
                ResourceType = "Water",
                Description = "100 bottles",
                Quantity = 100,
                DateDonated = DateTime.Now
            };

            _pageModel.Donation = newDonation;


            _pageModel.ModelState.AddModelError("Donation.DonorName", "The DonorName field is required.");


            var result = await _pageModel.OnPostAsync();


            Assert.IsInstanceOfType(result, typeof(PageResult));

            Assert.AreEqual(0, _context.Donations.Count());
        }
    }
}
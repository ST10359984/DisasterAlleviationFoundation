using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Pages.Donations
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Donation> Donations { get; set; }

        public async Task OnGetAsync()
        {
            Donations = await _context.Donations.ToListAsync();
        }
    }
}
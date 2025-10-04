using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisasterAlleviationFoundation.Pages.DisasterReports
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DisasterReport> Reports { get; set; }

        public async Task OnGetAsync()
        {
            Reports = await _context.DisasterReports.ToListAsync();
        }
    }
}
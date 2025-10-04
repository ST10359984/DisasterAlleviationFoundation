using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Pages.DisasterReports
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DisasterReport DisasterReport { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DisasterReports.Add(DisasterReport);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
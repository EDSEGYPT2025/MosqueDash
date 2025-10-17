using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MosqueDash.Data;
using MosqueDash.Data.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MosqueDash.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // --- THIS IS THE FIX ---
            // We remove the Enrollments property from validation because
            // a new course will never have enrollments.
            ModelState.Remove("Course.Enrollments");

            if (!ModelState.IsValid)
            {
                // --- DEBUGGING CODE ---
                // Find and print validation errors to the Output window
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();

                foreach (var error in errors)
                {
                    Debug.WriteLine($"Validation Error in {error.Key}: {error.Errors.FirstOrDefault()?.ErrorMessage}");
                }
                // --- END DEBUGGING CODE ---
                return Page();
            }

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}


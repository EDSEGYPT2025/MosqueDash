using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MosqueDash.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Attendances)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveStudentAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            var courseId = enrollment.CourseId;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = courseId });
        }

        public async Task<IActionResult> OnPostEvaluateAsync(int enrollmentId, string evaluation, bool isExcellent)
        {
            // We need to use FirstOrDefaultAsync when the key might be composite or not standard
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            enrollment.TeacherEvaluation = evaluation;
            enrollment.IsExcellent = isExcellent;
            await _context.SaveChangesAsync();

            // Redirect back to the same page (using the course ID from the enrollment)
            // This re-runs OnGetAsync and reloads all data correctly.
            return RedirectToPage(new { id = enrollment.CourseId });
        }
    }
}


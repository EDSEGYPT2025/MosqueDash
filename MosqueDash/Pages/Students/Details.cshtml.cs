using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosqueDash.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }
        public Dictionary<int, double> AttendancePercentages { get; set; } = new Dictionary<int, double>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // A comprehensive query to get the student with all related data needed for the page
            // استعلام شامل لجلب الطالب مع كل البيانات المرتبطة التي نحتاجها في الصفحة
            Student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Attendances)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Student == null)
            {
                return NotFound();
            }

            // Calculate attendance percentage for each course enrollment
            // حساب نسبة الحضور لكل دورة مسجل بها الطالب
            foreach (var enrollment in Student.Enrollments)
            {
                if (enrollment.Attendances != null && enrollment.Attendances.Any())
                {
                    int totalDays = enrollment.Attendances.Count;
                    int presentDays = enrollment.Attendances.Count(a => a.IsPresent);
                    double percentage = (double)presentDays / totalDays * 100;
                    AttendancePercentages[enrollment.Id] = percentage;
                }
                else
                {
                    AttendancePercentages[enrollment.Id] = 0; // No attendance recorded yet
                }
            }

            return Page();
        }
    }
}

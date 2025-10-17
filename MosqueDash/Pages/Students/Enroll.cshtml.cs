using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Data.Models;

namespace MosqueDash.Pages.Students
{
    public class EnrollModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EnrollModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public List<Course> AllCourses { get; set; } = new();
        public HashSet<int> EnrolledCourseIds { get; set; } = new();

        [BindProperty]
        public List<int> SelectedCourseIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            AllCourses = await _context.Courses.ToListAsync();
            EnrolledCourseIds = new HashSet<int>(await _context.Enrollments
                .Where(e => e.StudentId == id)
                .Select(e => e.CourseId)
                .ToListAsync());

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var studentToUpdate = await _context.Students
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.Id == Student.Id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            // Update student enrollments based on checkbox selection
            var currentEnrollments = studentToUpdate.Enrollments.Select(e => e.CourseId).ToList();
            var selectedCourses = SelectedCourseIds;

            // Courses to add
            var coursesToAdd = selectedCourses.Except(currentEnrollments);
            foreach (var courseId in coursesToAdd)
            {
                studentToUpdate.Enrollments.Add(new Enrollment { StudentId = studentToUpdate.Id, CourseId = courseId });
            }

            // Courses to remove
            var coursesToRemove = currentEnrollments.Except(selectedCourses);
            foreach (var courseId in coursesToRemove)
            {
                var enrollmentToRemove = studentToUpdate.Enrollments.FirstOrDefault(e => e.CourseId == courseId);
                if (enrollmentToRemove != null)
                {
                    _context.Enrollments.Remove(enrollmentToRemove);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

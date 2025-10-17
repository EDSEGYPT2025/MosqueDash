using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MosqueDash.Pages.Attendance
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? SelectedCourseId { get; set; }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; } = DateTime.Today;

        public SelectList CoursesList { get; set; }

        [BindProperty]
        public List<AttendanceViewModel> StudentsForAttendance { get; set; }

        public async Task OnGetAsync()
        {
            CoursesList = new SelectList(await _context.Courses.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");

            if (SelectedCourseId.HasValue)
            {
                var enrollments = await _context.Enrollments
                    .Include(e => e.Student)
                    .Where(e => e.CourseId == SelectedCourseId.Value)
                    .OrderBy(e => e.Student.Name)
                    .ToListAsync();

                StudentsForAttendance = new List<AttendanceViewModel>();

                foreach (var enrollment in enrollments)
                {
                    var existingAttendance = await _context.Attendances
                        .FirstOrDefaultAsync(a => a.EnrollmentId == enrollment.Id && a.Date == AttendanceDate.Date);

                    StudentsForAttendance.Add(new AttendanceViewModel
                    {
                        EnrollmentId = enrollment.Id,
                        StudentName = enrollment.Student.Name,
                        IsPresent = existingAttendance?.IsPresent ?? true // Default to Present
                    });
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate CoursesList if returning to the page with errors
                CoursesList = new SelectList(await _context.Courses.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
                return Page();
            }

            foreach (var studentAttendance in StudentsForAttendance)
            {
                var existingRecord = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EnrollmentId == studentAttendance.EnrollmentId && a.Date == AttendanceDate.Date);

                if (existingRecord != null)
                {
                    // Update existing record
                    existingRecord.IsPresent = studentAttendance.IsPresent;
                }
                else
                {
                    // Create new record
                    var newRecord = new Data.Models.Attendance
                    {
                        EnrollmentId = studentAttendance.EnrollmentId,
                        Date = AttendanceDate.Date,
                        IsPresent = studentAttendance.IsPresent
                    };
                    _context.Attendances.Add(newRecord);
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم حفظ سجل الحضور بنجاح!";
            return RedirectToPage(new { SelectedCourseId, AttendanceDate = AttendanceDate.ToString("yyyy-MM-dd") });
        }
    }

    public class AttendanceViewModel
    {
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; }
        public bool IsPresent { get; set; }
    }
}

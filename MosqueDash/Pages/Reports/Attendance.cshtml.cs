using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using AttendanceEntity = MosqueDash.Data.Models.Attendance;

namespace MosqueDash.Pages.Reports
{
    public class AttendanceModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AttendanceModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<AttendanceEntity> Attendances { get; set; }
        public SelectList CourseList { get; set; }

        // --- Filter Properties ---
        [BindProperty(SupportsGet = true)]
        public int? SelectedCourseId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }
        // -------------------------

        public async Task OnGetAsync(int? pageIndex)
        {
            CourseList = new SelectList(await _context.Courses.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");

            IQueryable<AttendanceEntity> attendanceIQ = _context.Attendances
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Student)
                .Include(a => a.Enrollment)
                    .ThenInclude(e => e.Course)
                .OrderByDescending(a => a.Date);

            // --- Apply Filters ---
            if (SelectedCourseId.HasValue && SelectedCourseId > 0)
            {
                attendanceIQ = attendanceIQ.Where(a => a.Enrollment.CourseId == SelectedCourseId);
            }

            // Apply StartDate filter
            if (StartDate.HasValue)
            {
                attendanceIQ = attendanceIQ.Where(a => a.Date.Date >= StartDate.Value.Date);
            }

            // Apply EndDate filter
            if (EndDate.HasValue)
            {
                attendanceIQ = attendanceIQ.Where(a => a.Date.Date <= EndDate.Value.Date);
            }
            // ---------------------

            int pageSize = 10;
            Attendances = await PaginatedList<AttendanceEntity>.CreateAsync(
                attendanceIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}


using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosqueDash.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; set; }
        public int ActiveCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalAttendanceRecords { get; set; }
        public List<Student> RecentStudents { get; set; }

        public async Task OnGetAsync()
        {
            TotalStudents = await _context.Students.CountAsync();
            ActiveCourses = await _context.Courses.CountAsync(); // Simplified logic for active courses
            TotalEnrollments = await _context.Enrollments.CountAsync();
            TotalAttendanceRecords = await _context.Attendances.CountAsync();

            RecentStudents = await _context.Students
                                        .OrderByDescending(s => s.Id)
                                        .Take(5)
                                        .ToListAsync();
        }
    }
}


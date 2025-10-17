using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Data.Models;

namespace MosqueDash.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public string CurrentSearch { get; set; }

        public async Task OnGetAsync()
        {
            var studentsQuery = _context.Students
                .Include(s => s.Enrollments)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                CurrentSearch = SearchTerm;
                studentsQuery = studentsQuery.Where(s => s.Name.Contains(SearchTerm)
                                                      || s.PhoneNumber.Contains(SearchTerm));
            }

            Students = await studentsQuery.OrderBy(s => s.Name).ToListAsync();
        }
    }
}


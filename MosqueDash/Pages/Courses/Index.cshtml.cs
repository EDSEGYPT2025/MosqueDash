using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data;
using MosqueDash.Data.Models;

namespace MosqueDash.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public string CurrentSearch { get; set; }

        public async Task OnGetAsync()
        {
            // Start with a queryable object. This doesn't fetch the data yet.
            var coursesQuery = _context.Courses
                .Include(c => c.Enrollments)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                CurrentSearch = SearchTerm;
                // Add a where clause to filter the results.
                coursesQuery = coursesQuery.Where(c => c.Name.Contains(SearchTerm)
                                                    || c.Department.Contains(SearchTerm));
            }

            // Execute the query and get the final list.
            Courses = await coursesQuery.ToListAsync();
        }
    }
}


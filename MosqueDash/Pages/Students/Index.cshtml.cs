using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MosqueDash.Data.Models;
using MosqueDash.Helpers;


namespace MosqueDash.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly MosqueDash.Data.ApplicationDbContext _context;
        private readonly IConfiguration Configuration;


        public IndexModel(MosqueDash.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }
        public EducationType? CurrentEducationType { get; set; }
        public EducationStage? CurrentEducationStage { get; set; }


        public PaginatedList<Student> Students { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex, EducationType? educationType, EducationStage? educationStage)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CurrentFilter = searchString;
            CurrentEducationType = educationType;
            CurrentEducationStage = educationStage;


            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.Name.Contains(searchString)
                                       || s.PhoneNumber.Contains(searchString));
            }

            if (educationType.HasValue)
            {
                studentsIQ = studentsIQ.Where(s => s.EducationType == educationType.Value);
            }

            if (educationStage.HasValue)
            {
                studentsIQ = studentsIQ.Where(s => s.EducationStage == educationStage.Value);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.Name);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.Name);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);
            Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}

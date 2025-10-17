using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MosqueDash.Data.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } // e.g., "Summer Quran Camp 2024"

        [StringLength(100)]
        public string Department { get; set; } // e.g., "Quran", "Tajweed"

        public string Curriculum { get; set; } // المنهج

        public string Manager { get; set; } // Manager/person in charge

        // The new property that needs to be added
        [Display(Name = "وصف الدورة")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "تاريخ البدء")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        // Navigation property for the many-to-many relationship
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}


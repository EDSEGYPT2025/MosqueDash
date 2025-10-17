using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MosqueDash.Data.Models
{
    /// <summary>
    /// يمثل جدول الطلاب في قاعدة البيانات
    /// </summary>
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        [Display(Name = "اسم الطالب")]
        public string Name { get; set; }

        [Display(Name = "العمر")]
        public int Age { get; set; }

        [StringLength(100)]
        [Display(Name = "اسم ولي الأمر")]
        public string GuardianName { get; set; }

        [Phone]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ملاحظات")]
        public string Notes { get; set; }

        // New property to track when the student was added
        [Display(Name = "تاريخ الإضافة")]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation property for the many-to-many relationship
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}


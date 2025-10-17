using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MosqueDash.Data.Models
{
    /// <summary>
    /// يمثل الجدول الوسيط لتسجيل طالب في دورة
    /// </summary>
    public class Enrollment
    {
        public int Id { get; set; }

        // Foreign Key for Student
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // Foreign Key for Course
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // --- Evaluation Fields ---
        [Display(Name = "طالب مميز")]
        public bool IsExcellent { get; set; } // هل الطالب مميز في هذه الدورة

        [Display(Name = "تقييم المدرس")]
        [StringLength(50)]
        public string? TeacherEvaluation { get; set; } // التقييم من المدرس (ممتاز, جيد جدا, etc.)

        [Display(Name = "ملاحظات التقييم")]
        [DataType(DataType.MultilineText)]
        public string? EvaluationNotes { get; set; } // A place for the teacher to write optional comments.
        // -------------------------

        // Navigation property for attendance records
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}


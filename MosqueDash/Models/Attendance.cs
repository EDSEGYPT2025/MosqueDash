using System;
using System.ComponentModel.DataAnnotations;

namespace MosqueDash.Data.Models
{
    /// <summary>
    /// Represents a single attendance record for a student in a specific course on a specific day.
    /// يمثل سجل حضور واحد لطالب في دورة معينة في يوم معين
    /// </summary>
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public bool IsPresent { get; set; } // true for present, false for absent
    }
}

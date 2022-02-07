using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot.Models
{
    [Table("attendance_students")]
    [Index(nameof(AttendanceId), Name = "attendance_id_attendance_students_idx")]
    [Index(nameof(StudentId), Name = "student_id_attendance_students_idx")]
    public partial class AttendanceStudent
    {
        [Key]
        [Column("attendance_student_id")]
        public int AttendanceStudentId { get; set; }
        [Column("attendance_id")]
        public int AttendanceId { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(AttendanceId))]
        [InverseProperty("AttendanceStudents")]
        public virtual Attendance Attendance { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty("AttendanceStudents")]
        public virtual Student Student { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot.Models
{
    [Table("attendances")]
    public partial class Attendance
    {
        public Attendance()
        {
            AttendanceStudents = new HashSet<AttendanceStudent>();
        }

        [Key]
        [Column("attendance_id")]
        public int AttendanceId { get; set; }
        [Required]
        [Column("class")]
        [StringLength(10)]
        public string Class { get; set; }
        [Column("session")]
        public DateTime Session { get; set; }
        [Column("status", TypeName = "tinyint")]
        public byte Status { get; set; }

        [InverseProperty(nameof(AttendanceStudent.Attendance))]
        public virtual ICollection<AttendanceStudent> AttendanceStudents { get; set; }
    }
}

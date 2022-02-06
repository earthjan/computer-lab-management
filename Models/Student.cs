using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot
{
    [Table("students")]
    public partial class Student
    {
        public Student()
        {
            AttendanceStudents = new HashSet<AttendanceStudent>();
        }

        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }
        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [Column("ip_address")]
        [StringLength(50)]
        public string IpAddress { get; set; }

        [InverseProperty(nameof(AttendanceStudent.Student))]
        public virtual ICollection<AttendanceStudent> AttendanceStudents { get; set; }
    }
}

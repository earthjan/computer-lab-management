using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RemoteScreenshot;
using RemoteScreenshot.CleanString;
using RemoteScreenshot.Models;

namespace RemoteScreenshot.Pages
{
    public class StudentModel : PageModel
    {
        private readonly ILogger<StudentModel> _logger;

        [BindProperty]
        public int attendanceId { get; set; }
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string password { get; set; }
        public string ErrorAttend { get; private set; } = "";
        public string SuccessAttend { get; private set; } = "";

        private RemoteDesktopContext remoteDesktopContext;

        public StudentModel(ILogger<StudentModel> logger, RemoteDesktopContext remoteDesktopContext)
        {
            _logger = logger;
            this.remoteDesktopContext = remoteDesktopContext;
        }

        public void OnGet()
        {
        }

        public void OnPostAttend()
        {
            bool isAttendanceExistAndOpen = remoteDesktopContext.IsAttendanceExistAndOpen(this.attendanceId);         

            if (isAttendanceExistAndOpen)
            {
                string username = RemoveSpecialCharacters.Remove(this.username);
                string password = RemoveSpecialCharacters.Remove(this.password);

                var student = remoteDesktopContext.GetStudent(username, password, Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());

                if (student != null)
                {
                    bool isStudentNotAttended = remoteDesktopContext.IsStudentNotAttended(student.StudentId, this.attendanceId);

                    if (isStudentNotAttended)
                    {
                        var insertedStudent = remoteDesktopContext.InsertAttendanceStudent(this.attendanceId, student.StudentId);
                        
                        if (insertedStudent == null)
                        {
                            this.ErrorAttend = "Invalid operation: Inserting the student to attendance_students is unsuccessful.";
                        }
                        else
                        {
                            this.SuccessAttend = $"Operation successful: You're attended in the session with the following information: [Attendance ID] {this.attendanceId} [Student ID] {student.StudentId} [Student username] {student.Username}.";

                        }
                    }
                    else
                    {
                        this.ErrorAttend = "Invalid operation: You're already attended in this session.";
                    }

                }
                else
                {
                    this.ErrorAttend = "Invalid operation: Your username, password, or laboratory unit is incorrect.";
                }
            }
            else
            {
                this.ErrorAttend = "Invalid operation: the attendance session is either closed or not existed.";
            }
        }
    }
}

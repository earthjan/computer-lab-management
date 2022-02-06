using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace RemoteScreenshot
{
    public partial class RemoteDesktopContext
    {
        private string ConnectionString = Environment.GetEnvironmentVariable(Configs.Variable, EnvironmentVariableTarget.Machine);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (this.ConnectionString == null)
                    throw new("Check your connection string.");

                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseMySQL(this.ConnectionString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // DML & QL
        public List<Desktop> GetAllRemoteDesktops()
        {
            try
            {
                var desktops = this.Desktops.FromSqlRaw("SELECT * FROM remote_desktop.desktops").ToList();

                return desktops;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public Student GetStudent(string username, string password, string ip)
        {
            try
            {
                Student student = this.Students
                .FromSqlRaw($"SELECT * FROM remote_desktop.students WHERE username = '{username}' AND password = '{password}' AND ip_address = '{ip}'")
                .Single();

                return student;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public List<Attendance> GetAllOpenAttendances()
        {
            try
            {
                var openAttendances = this.Attendances
                .FromSqlRaw("SELECT * FROM attendances WHERE status = 1")
                .ToList();

                return openAttendances;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public Desktop GetRemoteDesktop(string machineName)
        {

            try
            {
                var desktop = this.Desktops.FromSqlRaw($"SELECT * FROM remote_desktop.desktops WHERE name = \"{machineName}\"").Single();

                return desktop;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return new Desktop
            {
                DesktopId = 0,
                Name = "null",
                Username = "null",
                Password = "null",
                ScreenshotDirectory = "null",
                NircmdDirectory = "null",
                UserSession = 0,
                TasklistOutputDirectory = "null",
                Status = 0,
                IpAddress = "null"
            };
        }

        public Screenshot GetRemoteDesktopLatestScreenshot(int desktopId)
        {
            try
            {
                var screenshot = this.Screenshots
                .FromSqlRaw($"SELECT * FROM screenshots WHERE desktop_id = {desktopId} ORDER BY screenshots.timestamp DESC LIMIT 1")
                .Single();

                return screenshot;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            var now = DateTime.Now;
            return new Screenshot
            {
                ScreenshotId = 0,
                DesktopId = 0,
                Image = null,
                Timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)
            };
        }

        public AdminConfig GetAdminConfig()
        {
            try
            {
                var directory = this.AdminConfigs
                                    .FromSqlRaw("SELECT * FROM remote_desktop.admin_configs")
                                    .Single();

                return directory;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public string GetPsexecDirectory()
        {
            try
            {
                string directory = this.AdminConfigs
                    .FromSqlRaw("SELECT * FROM remote_desktop.admin_configs")
                    .Single()
                    .PsexecDirectory;

                return directory;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public string GetAdminConfigsScreenshotDirectory()
        {
            try
            {
                string directory = this.AdminConfigs
                                .FromSqlRaw("SELECT * FROM remote_desktop.admin_configs")
                                .Single()
                                .ScreenshotDirectory;

                return directory;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public List<Screenshot> GetAllRemoteDesktopLatestScreenshots()
        {
            try
            {
                List<Screenshot> directory = this.Screenshots
                    .FromSqlRaw("SELECT DISTINCT screenshots.desktop_id, screenshots.screenshot_id, screenshots.screenshot, screenshots.timestamp FROM remote_desktop.screenshots GROUP BY desktop_id, screenshots.screenshot_id, screenshots.screenshot, screenshots.timestamp ORDER BY screenshots.timestamp DESC")
                    .ToList();

                var latestScreenshots = new List<Screenshot>();
                latestScreenshots.Add(directory.First());

                int desktopId = directory.First().DesktopId;

                directory.ForEach((screenshot) =>
                {
                    if (desktopId != screenshot.DesktopId)
                    {
                        desktopId = screenshot.DesktopId;
                        latestScreenshots.Add(screenshot);
                    }
                });

                return latestScreenshots;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public List<Screenshot> GetLabLatestScreenshots(int labId)
        {
            try
            {
                // gets all desktop of a particular laboratory.
                var labDesktops = this.LaboratoryDesktops
                                .FromSqlRaw($"SELECT * FROM remote_desktop.laboratory_desktops WHERE laboratory_id = {labId}")
                                .ToList();

                var labDesktopLatestScreenshots = new List<Screenshot>();

                // gets the latest screenshots of the lab desktops based on the IDs of retrieved desktops.
                labDesktops.ForEach((labDesktop) =>
                {
                    labDesktopLatestScreenshots.Add(this.GetRemoteDesktopLatestScreenshot(labDesktop.DesktopId));
                });

                return labDesktopLatestScreenshots;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public List<Desktop> GetLabDesktops(int labId)
        {
            try
            {
                // gets all desktop of a particular laboratory.
                var labDesktopIds = this.LaboratoryDesktops
                                .FromSqlRaw($"SELECT * FROM remote_desktop.laboratory_desktops WHERE laboratory_id = {labId}")
                                .ToList();

                var labDesktops = new List<Desktop>();
                var allDesktops = this.GetAllRemoteDesktops();

                // gets the latest screenshots of the lab desktops based on the IDs of retrieved desktops.
                labDesktopIds.ForEach((labDesktopId) =>
                {
                    allDesktops.ForEach((desktop) =>
                    {
                        if (labDesktopId.DesktopId == desktop.DesktopId)
                            labDesktops.Add(desktop);
                    });
                });

                return labDesktops;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public AdminAccount GetAdminAccount(string username, string password)
        {
            try
            {
                var adminAccount = this.AdminAccounts
                    .FromSqlRaw($"SELECT * FROM remote_desktop.admin_accounts WHERE admin_accounts.username = \"{username}\" AND password = \"{password}\" ")
                    .Single();

                return adminAccount;
            }
            catch (InvalidCastException)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public Attendance UpdateAttendanceStatus(int attendanceId, byte status)
        {
            try
            {
                var attendance = this.Attendances
                    .FromSqlRaw($"SELECT * FROM attendances WHERE attendance_id = {attendanceId}")
                    .Single();

                if (attendance.Status == status)
                {
                    // means attendance is already opened.
                    return null;
                }

                attendance.Status = status;
                this.Attendances.Update(attendance);

                int affected = this.SaveChanges();

                if (affected < 1)
                    throw new("Updating the attendance status failed.");
                else
                    return attendance;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            // means no such attendance or operation failed.
            return null;
        }

        public bool UpdateOutputDeviceStatus(int desktopId, string outputDeviceStatus)
        {
            try
            {
                var remoteDesktop = this.Desktops
                .FromSqlRaw($"SELECT * FROM desktops WHERE desktops.desktop_id = {desktopId}")
                .Single();

                remoteDesktop.OutputDeviceStatus = outputDeviceStatus;

                this.Desktops.Update(remoteDesktop);

                int affected = this.SaveChanges();

                if (affected < 1)
                    throw new("Updating the remote desktop's output device status failed.");
                else
                    return true;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return false;
        }

        public bool UpdateRemoteDesktopUserSession(int desktopId, int newSessionNumber)
        {
            try
            {
                var remoteDesktop = this.Desktops
                    .FromSqlRaw($"SELECT * FROM desktops WHERE desktops.desktop_id = {desktopId}")
                    .Single();

                remoteDesktop.UserSession = newSessionNumber;

                this.Desktops.Update(remoteDesktop);

                int affected = this.SaveChanges();

                if (affected < 1)
                    throw new("Updating the remote desktop's user session failed.");
                else
                    return true;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return false;
        }

        public bool UpdateDesktopStatus(int desktopId, byte status)
        {
            try
            {
                var remoteDesktop = this.Desktops
                 .FromSqlRaw($"SELECT * FROM desktops WHERE desktops.desktop_id = {desktopId}")
                 .Single();

                remoteDesktop.Status = status;

                this.Desktops.Update(remoteDesktop);

                this.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return false;
        }

        public bool InsertScreenshot(int desktopId, string screenshotDirectory, string machineName)
        {
            try
            {
                var now = DateTime.Now;
                var screenshot = new Screenshot
                {
                    DesktopId = desktopId,
                    Image = File.ReadAllBytes($"{screenshotDirectory}{machineName}.jpg"),
                    Timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)
                };

                this.Screenshots.Add(screenshot);
                int affected = this.SaveChanges();

                if (affected < 1)
                    throw new("Inserting screenshot failed.");
                else
                    return true;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return false;
        }

        public Attendance InsertAttendance(string @class)
        {
            try
            {
                var now = DateTime.Now;
                var attendance = new Attendance
                {
                    Class = @class,
                    Session = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second),
                    Status = 1
                };

                this.Attendances.Add(attendance);
                int affected = this.SaveChanges();

                if (affected < 1)
                {
                    throw new("Inserting attendance failed.");
                }

                var insertedAttendance = this.Attendances
                    .FromSqlRaw("SELECT * FROM attendances ORDER BY attendance_id DESC LIMIT 1")
                    .Single();

                return insertedAttendance;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public AttendanceStudent InsertAttendanceStudent(int attendanceId, int studentId)
        {
            try
            {
                var now = DateTime.Now;
                var attendanceStudent = new AttendanceStudent
                {
                    AttendanceId = attendanceId,
                    StudentId = studentId,
                    Timestamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second),
                };

                this.AttendanceStudents.Add(attendanceStudent);
                int affected = this.SaveChanges();

                if (affected < 1)
                {
                    throw new("Inserting student for attendance failed.");
                }

                var insertedAttendanceStudent = this.AttendanceStudents
                    .FromSqlRaw("SELECT * FROM attendance_students ORDER BY attendance_student_id DESC LIMIT 1")
                    .Single();

                return insertedAttendanceStudent;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return null;
        }

        public bool IsAttendanceExistAndOpen(int attendanceId)
        {
            try
            {
                var attendance = this.Attendances
                    .FromSqlRaw($"SELECT * FROM attendances WHERE attendance_id = {attendanceId} AND status = 1")
                    .Single();

                return true;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return false;
        }

        public bool IsStudentNotAttended(int studentId, int attendanceId)
        {
            try
            {
                var attendanceStudent = this.AttendanceStudents
                    .FromSqlRaw($"SELECT * FROM attendance_students WHERE student_id = {studentId} AND attendance_id = {attendanceId}")
                    .Single();

                return false;
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            return true;
        }
    }
}

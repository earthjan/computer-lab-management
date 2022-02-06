using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RemoteScreenshot;
using RemoteScreenshot.CleanString;

namespace RemoteScreenshot.Pages
{
    public class HomeModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int LabId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MachineName { get; set; } = "";
        [BindProperty]
        public string Class { get; set; } = "no given class";
        [BindProperty]
        public int AttendanceId { get; set; } = 0;

        public List<Attendance> OpenAttendances { get; set; }
        public List<Desktop> LabDesktops { get; private set; } = new List<Desktop>();

        private RemoteDesktopContext remoteDesktopContext;

        public HomeModel(RemoteDesktopContext remoteDesktopContext)
        {
            this.remoteDesktopContext = remoteDesktopContext;
            this.OpenAttendances = remoteDesktopContext.GetAllOpenAttendances();
        }

        public void OnGet()
        {
        }

        public JsonResult OnGetShutdown()
        {
            object @object = null;
            string machineName = RemoveSpecialCharacters.Remove(this.MachineName);
            var desktop = remoteDesktopContext.GetRemoteDesktop(machineName);

            try
            {
                if (!machineName.Equals(""))
                {

                    Shutdown.RemotelyShutdown(
                        remoteDesktopContext.GetAdminConfig().PsshutdownDirectory,
                        desktop.Name,
                        desktop.Username,
                        desktop.Password);

                    @object = new
                    {
                        Message = $"{desktop.Name} was shutdown successfully."
                    };

                    return new JsonResult(@object);
                }
                else
                {
                    // choosing a desktop error message
                    ModelState.AddModelError(string.Empty, "Invalid machine name");
                }

            }
            catch (Exception ex)
            {
                // Info  
                Console.WriteLine(ex);
            }

            @object = new
            {
                Message = $"{desktop.Name} was failed to shutdown."
            };

            return new JsonResult(@object);
        }

        public IActionResult OnGetLabInitial()
        {
            try
            {
                if (this.LabId > 0)
                {
                    var labDesktops = remoteDesktopContext.GetLabDesktops(this.LabId);

                    if (labDesktops.Count != 0)
                    {
                        this.LabDesktops = labDesktops;
                    }
                    else
                    {
                        // choosing lab error message
                        ModelState.AddModelError(string.Empty, "Invalid lab id");
                    }

                }
                else
                {
                    // choosing lab error message
                    ModelState.AddModelError(string.Empty, "Invalid lab id");
                }

            }
            catch (Exception ex)
            {
                // Info  
                Console.WriteLine(ex.Message);
            }

            return Page();
        }

        public JsonResult OnGetLabInterval()
        {
            object objects;
            try
            {
                if (this.LabId > 0)
                {
                    var labDesktops = remoteDesktopContext.GetLabDesktops(this.LabId);

                    if (labDesktops.Count != 0)
                    {
                        objects = new
                        {
                            Desktops = labDesktops
                        };
                        return new JsonResult(objects);
                    }
                    else
                    {
                        // choosing lab error message
                        ModelState.AddModelError(string.Empty, "Invalid lab id");
                    }

                }
                else
                {
                    // choosing lab error message
                    ModelState.AddModelError(string.Empty, "Invalid lab id");
                }

            }
            catch (Exception ex)
            {
                // Info  
                Console.WriteLine(ex.Message);
            }

            objects = new
            {
                Desktops = new List<Desktop>(),
            };

            return new JsonResult(objects);
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                // Setting.  
                var authenticationManager = Request.HttpContext;

                // Sign Out.  
                await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Info.  
            return this.RedirectToPage("/Index");
        }

        public JsonResult OnPostAttendance()
        {
            string @class = RemoveSpecialCharacters.Remove(this.Class);

            var attendance = remoteDesktopContext.InsertAttendance(@class);

            return new JsonResult(new { Attendance = attendance });
        }

        public JsonResult OnPostReopen()
        {
            var attendance = remoteDesktopContext.UpdateAttendanceStatus(this.AttendanceId, 1);

            return new JsonResult(new { Attendance = attendance });
        }

        public JsonResult OnPostClose()
        {
            var attendance = remoteDesktopContext.UpdateAttendanceStatus(this.AttendanceId, 0);

            return new JsonResult(new { Attendance = attendance });
        }
    }
}

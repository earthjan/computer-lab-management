using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot.Models
{
    [Table("desktops")]
    [Index(nameof(Name), Name = "name_UNIQUE", IsUnique = true)]
    public partial class Desktop
    {
        public Desktop()
        {
            LaboratoryDesktops = new HashSet<LaboratoryDesktop>();
            Screenshots = new HashSet<Screenshot>();
        }

        [Key]
        [Column("desktop_id")]
        public int DesktopId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [Column("screenshot_directory")]
        [StringLength(255)]
        public string ScreenshotDirectory { get; set; }
        [Required]
        [Column("nircmd_directory")]
        [StringLength(255)]
        public string NircmdDirectory { get; set; }
        [Column("user_session")]
        public int UserSession { get; set; }
        [Required]
        [Column("tasklist_output_directory")]
        [StringLength(255)]
        public string TasklistOutputDirectory { get; set; }
        [Column("status", TypeName = "tinyint")]
        public byte Status { get; set; }
        [Required]
        [Column("ip_address")]
        [StringLength(50)]
        public string IpAddress { get; set; }
        [Required]
        [Column("output_device_status")]
        [StringLength(255)]
        public string OutputDeviceStatus { get; set; }
        [Required]
        [Column("output_device_monitoring_script_directory")]
        [StringLength(255)]
        public string OutputDeviceMonitoringScriptDirectory { get; set; }
        [Required]
        [Column("currently_running_apps", TypeName = "longtext")]
        public string CurrentlyRunningApps { get; set; }
        [Required]
        [Column("app_monitoring_script_directory")]
        [StringLength(255)]
        public string AppMonitoringScriptDirectory { get; set; }

        [InverseProperty(nameof(LaboratoryDesktop.Desktop))]
        public virtual ICollection<LaboratoryDesktop> LaboratoryDesktops { get; set; }
        [InverseProperty(nameof(Screenshot.Desktop))]
        public virtual ICollection<Screenshot> Screenshots { get; set; }
    }
}

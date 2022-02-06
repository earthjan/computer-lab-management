using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot
{
    [Table("admin_configs")]
    public partial class AdminConfig
    {
        [Key]
        [Column("admin_config_id")]
        public int AdminConfigId { get; set; }
        [Required]
        [Column("psexec_directory")]
        [StringLength(50)]
        public string PsexecDirectory { get; set; }
        [Required]
        [Column("psshutdown_directory")]
        [StringLength(50)]
        public string PsshutdownDirectory { get; set; }
        [Required]
        [Column("screenshot_directory")]
        [StringLength(255)]
        public string ScreenshotDirectory { get; set; }
    }
}

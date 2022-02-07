using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot.Models
{
    [Table("screenshots")]
    [Index(nameof(DesktopId), Name = "desktop_id_idx")]
    public partial class Screenshot
    {
        [Key]
        [Column("screenshot_id")]
        public int ScreenshotId { get; set; }
        [Column("desktop_id")]
        public int DesktopId { get; set; }
        [Required]
        [Column("image", TypeName = "mediumblob")]
        public byte[] Image { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(DesktopId))]
        [InverseProperty("Screenshots")]
        public virtual Desktop Desktop { get; set; }
    }
}

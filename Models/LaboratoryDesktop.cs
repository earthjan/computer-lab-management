using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot.Models
{
    [Table("laboratory_desktops")]
    [Index(nameof(DesktopId), Name = "desktop_id_idx")]
    [Index(nameof(LaboratoryId), Name = "laboratory_id_laboratory_desktops_idx")]
    public partial class LaboratoryDesktop
    {
        [Key]
        [Column("laboratory_desktop_id")]
        public int LaboratoryDesktopId { get; set; }
        [Column("laboratory_id")]
        public int LaboratoryId { get; set; }
        [Column("desktop_id")]
        public int DesktopId { get; set; }

        [ForeignKey(nameof(DesktopId))]
        [InverseProperty("LaboratoryDesktops")]
        public virtual Desktop Desktop { get; set; }
        [ForeignKey(nameof(LaboratoryId))]
        [InverseProperty("LaboratoryDesktops")]
        public virtual Laboratory Laboratory { get; set; }
    }
}

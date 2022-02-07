using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace RemoteScreenshot.Models
{
    [Table("laboratories")]
    public partial class Laboratory
    {
        public Laboratory()
        {
            LaboratoryDesktops = new HashSet<LaboratoryDesktop>();
        }

        [Key]
        [Column("laboratory_id")]
        public int LaboratoryId { get; set; }

        [InverseProperty(nameof(LaboratoryDesktop.Laboratory))]
        public virtual ICollection<LaboratoryDesktop> LaboratoryDesktops { get; set; }
    }
}

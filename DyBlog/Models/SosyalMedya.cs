namespace DyBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SosyalMedya")]
    public partial class SosyalMedya
    {
        [Key]
        public int sosId { get; set; }

        [StringLength(50)]
        public string Adi { get; set; }

        [StringLength(250)]
        public string UrlAdres { get; set; }

        [StringLength(50)]
        public string sosClass { get; set; }
    }
}

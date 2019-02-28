namespace DyBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hakkimda")]
    public partial class Hakkimda
    {
        [Key]
        public int hak_id { get; set; }

        [StringLength(250)]
        public string icerik { get; set; }
    }
}

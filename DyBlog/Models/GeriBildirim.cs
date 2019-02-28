namespace DyBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeriBildirim")]
    public partial class GeriBildirim
    {
        public int id { get; set; }

        [StringLength(50)]
        public string Konu { get; set; }

        [StringLength(350)]
        public string Mesaj { get; set; }

        public int? kul_id { get; set; }

        public DateTime? Tarih { get; set; }

        public virtual Uye Uye { get; set; }
    }
}

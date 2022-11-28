using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppOppg2.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public Guid? id { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string firstName { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string lastName { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string username { get; set; }
        public byte[] passwordHashed { get; set; }

        [NotMapped]
        public string password { get; set; }

        public byte[] salt { get; set; }
    }
}
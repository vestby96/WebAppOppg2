using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppOppg2.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public Guid? Id { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string FirstName { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string LastName { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string Username { get; set; }
        public byte[] PasswordHashed { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public byte[] Salt { get; set; }
    }
}
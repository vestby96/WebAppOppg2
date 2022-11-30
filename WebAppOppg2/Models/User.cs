using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppOppg2.Models
{
    public class User
    { //get og set med regix begrensinger på hva som er tilatt å legge inn


        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   //sesifiserer at verdien vil bli generert av databasen ved en INSERT
        [Key, Column(Order = 0)]                                //spesifiserer column order
        public Guid? Id { get; set; }                           //universielt unik indikator

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string FirstName { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string LastName { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string Username { get; set; }
        public byte[] PasswordHashed { get; set; }

        [NotMapped]  //hvis vi ikke ønsker å vise den i database
        public string Password { get; set; }

        public byte[] Salt { get; set; } 
    }
}
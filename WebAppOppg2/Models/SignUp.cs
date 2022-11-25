using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppOppg2.Models
{
    public class User
    {
        public int usid { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string firstName { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string lastName { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string username { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string password { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,40}")]
        public string email { get; set; }
    }
}
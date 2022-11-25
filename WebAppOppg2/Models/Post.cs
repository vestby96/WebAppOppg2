using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppOppg2.Models
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime DateOccured { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string Country { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string City { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string Address { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string Shape { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,300}")]
        public string Summary { get; set; }
    }
}
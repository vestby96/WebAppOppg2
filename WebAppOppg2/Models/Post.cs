using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppOppg2.Models
{
    public class Post
    {
        public int id { get; set; }
        public string datePosted { get; set; }
        public DateTime dateOccured { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string country { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string city { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string address { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string shape { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,300}")]
        public string summary { get; set; }
    }
}
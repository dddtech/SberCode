using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PackageReview
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string GpUrl { get; set; }
        public Package Package { get; set; }
        public Category Category { get; set; }
        public long Timestamp { get; set; }
        public int Positive { get; set; }
    }
}

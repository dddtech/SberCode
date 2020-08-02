using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class CategoryCount
    {
        public Category Category { get; set; }
        public int Count { get; set; }
        public Package Package { get; set; }
    }
}

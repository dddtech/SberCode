using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Category
    {
        //public static Category Base = new Category() { Name="Без категории",Identificator=-1};
        public int Id { get; set; }
        public string Name { get; set; }
        public int Identificator { get; set; }
    }
}

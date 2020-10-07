using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Models
{
    public class Category
    {
        public int Id { set; get; }
        [Display(Name="Category")]
        public string CategoryName { set; get; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.ViewModels
{
    public class PostViewModel
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Body { set; get; }
        [Display(Name = "Background Image")]
        public IFormFile Image { set; get; }
        public string CategoryName { set; get; }
    }
}

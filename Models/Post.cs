using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Models
{
    public class Post
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Body { set; get; }
        public string Image { set; get; }
        public DateTime Created { set; get; } = DateTime.Now;
    }
}

﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.ViewModels
{
    public class PostViewModel
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Body { set; get; }
        public IFormFile Image { set; get; }
    }
}
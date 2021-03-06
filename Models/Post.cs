﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace BlogMVC.Models
{
    public class Post
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Body { set; get; }
        public string Image { set; get; }
        public DateTime Created { set; get; } = TimeZoneInfo
            .ConvertTime(DateTime.UtcNow, TZConvert.GetTimeZoneInfo("New Zealand Standard Time"));

        [Display(Name = "Category")]
        public string CategoryName { set; get; }
    }
}

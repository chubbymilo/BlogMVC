﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;

namespace BlogMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlogMVC.Models.Post> Post { get; set; }
        public DbSet<BlogMVC.Models.TodoItem> TodoItem { get; set; }
        public DbSet<BlogMVC.Models.Category> Category { get; set; }
    }
}

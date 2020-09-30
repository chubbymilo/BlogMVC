using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Data.Repository
{
    public class Repository:IRepository

    {
        private ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Post GetPost(int id)
        {
            return _context.Post.FirstOrDefault(p => p.Id == id);
        }
        public List<Post> GetAllPosts()
        {
            return _context.Post.ToList();
        }
        public void AddPost(Post post) {
            _context.Post.Add(post);
        }

        public void RemovePost(int id)
        {
            _context.Post.Remove(GetPost(id));

        }
        public void UpdatePost(Post post)
        {
            _context.Post.Update(post);
        }
        public async Task<bool> SaveChangeAsync()
        {
            if(await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

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
        public List<string> GetCategories()
        {
            List<Category> categories = _context.Category.ToList();
            List<String> allCategoris = new List<string>();
            foreach( var item in categories)
            {
                allCategoris.Add(item.CategoryName);
            }
            return allCategoris;
        }

        public List<Post> GetAllPostsWithCategory(string category)
        {

            return _context.Post
                .Where(p => p.CategoryName == category).ToList();
        }
    }
}

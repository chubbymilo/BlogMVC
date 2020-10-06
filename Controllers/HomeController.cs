using BlogMVC.Data.FileManager;
using BlogMVC.Data.Repository;
using BlogMVC.Models;
using BlogMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository _context;
        private IFileManager _fileManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IRepository context, IFileManager fileManager,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_context.GetAllPosts());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Post()
        {
            return View();
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            List<string> allCategorie =_context.GetCategories();
            ViewData["categories"] = allCategorie;
            return View();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PostViewModel postVm)
        {
            var post = new Post
            {
                Id = postVm.Id,
                Body = postVm.Body,
                Description = postVm.Description,
                Title = postVm.Title,
                Image = await _fileManager.SaveImage(postVm.Image),
                CategoryName = postVm.CategoryName
            };
            _context.AddPost(post);
            if (await _context.SaveChangeAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(post);
            }

        }
        public IActionResult List()
        {
            return View(_context.GetAllPosts());
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Post post = _context.GetPost(id);
            ViewData["imagePath"] = post.Image;
            List<string> allCategorie = _context.GetCategories();
            ViewData["categories"] = allCategorie;

            var newPost = new PostViewModel
            {
                Id = post.Id,
                Description = post.Description,
                Title = post.Title,
                Body = post.Body,
                CategoryName = post.CategoryName
            };
            return View(newPost);
        }
        [HttpPost, Authorize(Roles = "Admin")]
       public async Task<IActionResult> Edit(PostViewModel postVm)
        {
            Post old_post = _context.GetPost(postVm.Id);
            if (!postVm.Title.Equals(old_post.Title))
            {
                old_post.Title = postVm.Title;
            }
            if (!postVm.Description.Equals(old_post.Description))
            {
                old_post.Description = postVm.Description;
            }
            if (!postVm.Body.Equals(old_post.Body))
            {
                old_post.Body = postVm.Body;
            }
            if (!postVm.CategoryName.Equals(old_post.CategoryName))
            {
                old_post.CategoryName = postVm.CategoryName;
            }
            if (postVm.Image != null)
            {
                old_post.Image = await _fileManager.SaveImage(postVm.Image);
            }
            _context.UpdatePost(old_post);
            if (await _context.SaveChangeAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(postVm);
            }
        }

        public IActionResult Details(int id)
        {
            return View(_context.GetPost(id));
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            return View(_context.GetPost(id));
        }

        [HttpPost, ActionName("Delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string path = _context.GetPost(id).Image;
            _context.RemovePost(id);
            
            if (await _context.SaveChangeAsync())
            {
                _fileManager.RemoveImage(path);
                return RedirectToAction("Index");
            }
            else
            {
                return View(_context.GetPost(id));
            }
        }
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.IndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }

    }
}

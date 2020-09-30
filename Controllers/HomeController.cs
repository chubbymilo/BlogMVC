using BlogMVC.Data.FileManager;
using BlogMVC.Data.Repository;
using BlogMVC.Models;
using BlogMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository _context;
        private IFileManager _fileManager;

        public HomeController(ILogger<HomeController> logger, IRepository context, IFileManager fileManager)
        {
            _logger = logger;
            _context = context;
            _fileManager = fileManager;


        }

        public IActionResult Index()
        {
            return View();
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
            return View();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PostViewModel postVm)
        {
            var post = new Post
            {
                Id = postVm.Id,
                Body = postVm.Body,
                Title = postVm.Title,
                Image = await _fileManager.SaveImage(postVm.Image)
            };
            _context.AddPost(post);
            if (await _context.SaveChangeAsync())
            {
                return RedirectToAction("List");
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Post post = _context.GetPost(id);
            ViewData["imagePath"] = post.Image;

            var newPost = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
            };
            return View(newPost);
        }
        [HttpPost]
       public async Task<IActionResult> Edit(PostViewModel postVm)
        {
            Post post = new Post
            {
                Id = postVm.Id,
                Title = postVm.Title,
                Body = postVm.Body,
                Image = await _fileManager.SaveImage(postVm.Image)
            };
            _context.UpdatePost(post);
            if (await _context.SaveChangeAsync())
            {
                return RedirectToAction("List");
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_context.GetPost(id));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _context.RemovePost(id);
            if (await _context.SaveChangeAsync())
            {
                return RedirectToAction("List");
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

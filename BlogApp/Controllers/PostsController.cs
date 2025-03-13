using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IActionResult> Index(string? tag)
        {
            IQueryable<Post> posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(p => p.Tags.Any(t => t.Url == tag));
            }
            return View(
                new PostsDto
                {
                    Posts = await posts.Include(p => p.Tags).ToListAsync(),
                }
            );
        }

        public async Task<IActionResult> Details(string? url)
        {
            return View(await _postRepository
                                .Posts
                                .Include(p => p.Tags)
                                .Include(p => p.Comments)
                                .ThenInclude(p => p.User)
                                .FirstOrDefaultAsync(p => p.Url == url));
        }
    }
}
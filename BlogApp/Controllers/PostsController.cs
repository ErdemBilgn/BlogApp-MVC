using System.Security.Claims;
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
        private ICommentRepository _commentRepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index(string? tag)
        {
            var claims = User.Claims;

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

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            string? avatar = User.FindFirstValue(ClaimTypes.UserData);

            Comment entity = new Comment
            {
                PostId = PostId,
                Text = Text,
                PublishedOn = DateTime.Now,
                UserId = int.Parse(userId ?? "")
            };
            _commentRepository.CreateComment(entity);

            return Json(new
            {
                userName,
                Text,
                entity.PublishedOn,
                avatar
            });
        }
    }
}
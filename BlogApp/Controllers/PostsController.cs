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
        public JsonResult AddComment(int PostId, string UserName, string Text)
        {
            Comment entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                User = new User { UserName = UserName, Image = "avatar.jpg" }
            };
            _commentRepository.CreateComment(entity);

            return Json(new
            {
                UserName,
                Text,
                entity.PublishedOn,
                entity.User.Image
            });
        }
    }
}
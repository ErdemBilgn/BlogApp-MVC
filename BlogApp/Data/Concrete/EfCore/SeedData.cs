using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void SeedTestDatas(IApplicationBuilder app)
        {
            BlogContext? context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "web programming", Url = "web-programming", Color = TagColors.warning },
                        new Tag { Text = "backend", Url = "backend", Color = TagColors.danger },
                        new Tag { Text = "frontend", Url = "frontend", Color = TagColors.success },
                        new Tag { Text = "fullstack", Url = "fullstack", Color = TagColors.secondary },
                        new Tag { Text = "nodejs", Url = "nodejs", Color = TagColors.primary }
                    );

                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "erdembilgin", Image = "p1.jpg" },
                        new User { UserName = "helinsarac", Image = "p2.jpg" }
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "Asp.net core",
                            Content = "Asp.net core dersleri",
                            Url = "aspnet-core",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            UserId = 1,
                            Comments = new List<Comment>{
                                new Comment { Text = "a good course", PublishedOn = new DateTime(), UserId = 1},
                                new Comment { Text = "a very nice course", PublishedOn = new DateTime(), UserId = 2}
                            }
                        },

                        new Post
                        {
                            Title = "Php",
                            Content = "Php core dersleri",
                            Url = "php",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "2.jpg",
                            UserId = 1,
                        },

                        new Post
                        {
                            Title = "Django",
                            Content = "Django dersleri",
                            Url = "django",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-30),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "3.jpg",
                            UserId = 2,
                        },

                        new Post
                        {
                            Title = "Unity3D",
                            Content = "Unity3D dersleri",
                            Url = "unity3d",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-40),
                            Tags = context.Tags.Take(1).ToList(),
                            Image = "1.jpg",
                            UserId = 2,
                        },

                        new Post
                        {
                            Title = "React",
                            Content = "React dersleri",
                            Url = "react",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-50),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "2.jpg",
                            UserId = 1,
                        },

                        new Post
                        {
                            Title = "Unreal Engine",
                            Content = "Unreal Engine dersleri",
                            Url = "unreal-engine",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-60),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "1.jpg",
                            UserId = 2,
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
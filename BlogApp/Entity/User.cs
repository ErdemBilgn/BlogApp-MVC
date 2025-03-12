using System;

namespace BlogApp.Entity;

public class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public List<Post> Posts { get; set; } = [];

    public List<Comment> Comments { get; set; } = [];
}

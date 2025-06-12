using System;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class EntityDBContext(DbContextOptions<EntityDBContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

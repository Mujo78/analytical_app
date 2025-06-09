using System;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Data;

public class EntityDBContext(DbContextOptions<EntityDBContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

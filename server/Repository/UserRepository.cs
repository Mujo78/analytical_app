using System;
using server.Data;
using server.Models;
using server.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace server.Repository;

public class UserRepository : IUserRepository
{
    private readonly EntityDBContext _context;
    public UserRepository(EntityDBContext context)
    {
        _context = context;
    }
    public async Task<List<User>> GetTopReputationUsersAsync()
    {

        return await _context.Users
    .Where(u => u.Reputation > 0 &&
                EF.Functions.DateDiffDay(u.CreationDate, u.LastAccessDate) > 0)
    .OrderByDescending(u =>
        (double)u.Reputation / EF.Functions.DateDiffDay(u.CreationDate, u.LastAccessDate))
    .Take(10)
    .ToListAsync();

    }
}

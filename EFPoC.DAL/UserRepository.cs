using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFPoC.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EFPoC.DAL;
public class UserRepository : IRepository<User>
{
    private readonly MyDbContext _dbContext;
    public MyDbContext DbContext => _dbContext;

    public UserRepository(MyDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IList<User>> GetAllAsync(CancellationToken ct = default) {
        return await _dbContext.Users.ToListAsync(ct);
    }

    public async Task<IList<int>> GetAllIds(CancellationToken ct = default) {
        return await _dbContext.Users.Select(u => u.Id).ToListAsync(ct);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) {
        return await _dbContext.Users.FindAsync(id, ct);
    }

    public async Task<User> AddAsync(User entity, CancellationToken ct = default) {
        return (await _dbContext.Users.AddAsync(entity, ct)).Entity;
    }

    public Task DeleteAsync(User entity, CancellationToken _ = default) {
        _dbContext.Users.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<IList<User>> FindUsersWithCommentsAsync(int minCount = 1,
        CancellationToken ct = default) {
        return await _dbContext.Users.Where(u => u.Comments.Count >= minCount).ToListAsync(ct);
    }
}

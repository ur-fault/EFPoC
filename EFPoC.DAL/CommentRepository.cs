using EFPoC.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EFPoC.DAL;
public class CommentRepository : IRepository<Comment>
{
    private readonly MyDbContext _dbContext;
    public MyDbContext DbContext => _dbContext;

    public CommentRepository(MyDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<IList<Comment>> GetAllAsync(CancellationToken ct = default) {
        return await _dbContext.Comments.ToListAsync(ct);
    }

    public async Task<Comment?> GetByIdAsync(int id, CancellationToken ct = default) {
        return await _dbContext.Comments.Include(c => c.User).Where(c => c.Id == id).FirstOrDefaultAsync(ct);
    }

    public async Task<Comment> AddAsync(Comment entity, CancellationToken ct = default) {
        return (await _dbContext.Comments.AddAsync(entity, ct)).Entity;
    }

    public Task DeleteAsync(Comment entity, CancellationToken ct = default) {
        _dbContext.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<IList<Comment>> FindByUserAsync(int userId, CancellationToken ct = default) {
        return await _dbContext.Comments.Where(c => c.Id == userId).Include(c => c.User).ToListAsync(ct);
    }

    public async Task<IList<Comment>> FindOlderThanAsync(DateTimeOffset dateTimeOffset,
        CancellationToken ct = default) {
        return await _dbContext.Comments.Where(c => c.CreatedAt < dateTimeOffset.Date).Include(c => c.User).ToListAsync(ct);
    }

    public Task DeleteRangeAsync(IEnumerable<Comment> comments, CancellationToken _ = default) {
        _dbContext.Comments.RemoveRange(comments);

        return Task.CompletedTask;
    }

    public async Task<int> PurgeOlderThanAsync(DateTimeOffset dateTimeOffset,
        CancellationToken ct = default) {
        var comments = _dbContext.Comments.Where(c => c.CreatedAt < dateTimeOffset);
        _dbContext.Comments.RemoveRange(comments);

        return await comments.CountAsync(ct);
    }
}

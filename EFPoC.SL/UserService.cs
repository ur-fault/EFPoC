using EFPoC.DAL;
using EFPoC.DAL.Models;
using Microsoft.Extensions.Logging;

namespace EFPoC.SL;
public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(ILogger<UserService> logger, IUnitOfWork unitOfWork) {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default) {
        _logger.LogInformation("Getting user with id {Id}.", id);
        return await _unitOfWork.Users.GetByIdAsync(id: id, ct);
    }

    public async Task<IList<int>> GetAllIds(CancellationToken ct = default) {
        return await _unitOfWork.Users.GetAllIds(ct);
    }

    public async Task AddUserAsync(User user, CancellationToken ct = default) {
        _logger.LogInformation("Adding user with id {Id}.", user.Id);
        await _unitOfWork.Users.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<IList<User>> GetUsersWithCommentsAsync(int minCount = 1, CancellationToken ct = default) {
        _logger.LogInformation("Getting users with at least {MinCount} comments.", minCount);
        return await _unitOfWork.Users.FindUsersWithCommentsAsync(minCount, ct);
    }

    public async Task AddCommentAsync(Comment comment, bool dontSave = false, CancellationToken ct = default) {
        _logger.LogInformation("Adding comment to user with id {Id}.", comment.UserId);
        await _unitOfWork.Comments.AddAsync(comment, ct);
        if (!dontSave)
            await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<int> PurgeCommentsAsync(TimeSpan minAge, CancellationToken ct = default) {
        _logger.LogInformation("Purging comments.");
        var count = await _unitOfWork.Comments.PurgeOlderThanAsync(DateTimeOffset.Now.Add(minAge), ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return count;
    }

    public async Task SaveChangesAsync(CancellationToken ct) {
        await _unitOfWork.SaveChangesAsync(ct);
    }
}

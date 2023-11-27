using EFPoC.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EFPoC.DAL;

public interface IUnitOfWork
{
    MyDbContext MyDbContext { get; }

    UserRepository Users { get; }
    CommentRepository Comments { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
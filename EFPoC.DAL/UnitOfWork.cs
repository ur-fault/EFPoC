using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFPoC.DAL;
public class UnitOfWork : IUnitOfWork
{
    public MyDbContext MyDbContext { get; }

    public UserRepository Users { get; }
    public CommentRepository Comments { get; }

    public UnitOfWork(MyDbContext dbContext, UserRepository userRepository, CommentRepository comments) {
        MyDbContext = dbContext;
        Users = userRepository;
        Comments = comments;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) {
        return MyDbContext.SaveChangesAsync(ct);
    }
}

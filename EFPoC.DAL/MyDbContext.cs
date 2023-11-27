using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFPoC.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EFPoC.DAL;
public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    const string ConnectionString = "Server=.\\sqlexpress;Database=efpoc;User=sa;TrustServerCertificate=True";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(ConnectionString, builder => {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
        });
    }
}

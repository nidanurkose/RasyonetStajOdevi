using Microsoft.EntityFrameworkCore;
using RasyonetStaj.Models;

namespace RasyonetStaj.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Stock> Stocks { get; set; }
}
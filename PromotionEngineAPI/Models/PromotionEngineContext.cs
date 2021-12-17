using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace PromotionEngineAPI.Models;
public class PromotionEngineContext : DbContext
{
    public PromotionEngineContext(DbContextOptions<PromotionEngineContext> options) : base(options)
    {
    }

    public DbSet<PromotionEngineItem> PromotionEngineItems { get; set; } = null;

}
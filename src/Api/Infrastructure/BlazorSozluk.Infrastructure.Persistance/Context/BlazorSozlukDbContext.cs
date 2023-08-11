using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorSozluk.Api.Domain.Models;

namespace BlazorSozluk.Infrastructure.Persistance.Context;

public class BlazorSozlukDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "dbo";

    public BlazorSozlukDbContext()
    {

    }

    public BlazorSozlukDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Entry> Entries { get; set; }
    public DbSet<EntryFavorite> EntryFavorites { get; set; }
    public DbSet<EntryVote> EntryVotes { get; set; }
    public DbSet<EntryComment> EntryComments { get; set; }

    public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }
    public DbSet<EntryCommentVote> EntryCommentVotes { get; set; }
    public DbSet<EmailConfirmation> EmailConfirmations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //var connStr = "Data Source=DESKTOP-7CORDPF;Initial Catalog=shoppinglist;Persist Security Info=True;User ID=ba;Password=Berkan123456";
            //optionsBuilder.UseSqlServer(connStr, opt =>
            //{
            //    opt.EnableRetryOnFailure();
            //}); 

            //It Works In Design Time When We Adding Migration.

            var connStr = "Server=(localdb)\\MSSQLLocalDB;Database=ecommerce;Trusted_Connection=True";
            optionsBuilder.UseSqlServer(connStr, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSave()
    {
        var addedEntities = ChangeTracker.Entries()
                                .Where(i => i.State == EntityState.Added)
                                .Select(i => (BaseEntity)i.Entity);
        PrepareAddedEntities(addedEntities);
    }

    private void PrepareAddedEntities(IEnumerable<BaseEntity> baseEntities)
    {
        foreach (var entity in baseEntities)
        {
            if (entity.CreateDate == DateTime.MinValue)
                entity.CreateDate = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GoldDiggerWebApi.Models;

namespace GoldDiggerWebApi.Models;

public partial class GoldDiggerDbContext : DbContext
{
    public GoldDiggerDbContext()
    {
    }

    public GoldDiggerDbContext(DbContextOptions<GoldDiggerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<PreciousMetal> PreciousMetals { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GoldDiggerDB; Integrated Security=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Customers");

            entity.Property(e => e.Adresse).HasMaxLength(150);
            entity.Property(e => e.City).HasMaxLength(75);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.ContactName).HasMaxLength(75);
            entity.Property(e => e.ContactTitle).HasMaxLength(75);
            entity.Property(e => e.MailAdr).HasMaxLength(100);
            entity.Property(e => e.PhoneNo).HasMaxLength(30);
            entity.Property(e => e.Zip).HasMaxLength(25);
        });

        modelBuilder.Entity<PreciousMetal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PreciousMetals");

            entity.Property(e => e.MetalName).HasMaxLength(25);
            entity.Property(e => e.PricePerKg).HasColumnType("decimal(18, 6)");
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Rates");

            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Rates).HasColumnType("decimal(18, 12)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<GoldDiggerWebApi.Models.ClassApiResponds>? ClassApiResponds { get; set; }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CompanyWebApp.Models;

public partial class ItcompanyDbContext : DbContext
{
    public ItcompanyDbContext()
    {
    }

    public ItcompanyDbContext(DbContextOptions<ItcompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Citizen> Citizens { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Programmer> Programmers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= (LocalDb)\\MSSQLLocalDB;\nDatabase=ITCompanyDB; Trusted_Connection=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Education).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Citizens)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citizens_Countries");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Edrpou).HasColumnName("EDRPOU");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Header).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Companies_Countries");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Capital).HasMaxLength(50);
            entity.Property(e => e.Continent).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.Gdp).HasColumnName("GDP");
            entity.Property(e => e.Header).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ReleaseDate).HasMaxLength(50);
            entity.Property(e => e.Version).HasMaxLength(50);

            entity.HasOne(d => d.Company).WithMany(p => p.Products)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Companies");
        });

        modelBuilder.Entity<Programmer>(entity =>
        {
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Place).HasMaxLength(50);
            entity.Property(e => e.Range).HasMaxLength(50);
            entity.Property(e => e.Specialization).HasMaxLength(50);
            entity.Property(e => e.Time).HasMaxLength(50);

            entity.HasOne(d => d.Citizen).WithMany(p => p.Programmers)
                .HasForeignKey(d => d.CitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Programmers_Citizens");

            entity.HasOne(d => d.Company).WithMany(p => p.Programmers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Programmers_Companies");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

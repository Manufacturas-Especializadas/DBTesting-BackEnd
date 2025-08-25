using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBTesting.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Department { get; set; }

    public virtual DbSet<Employees> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC0788E6AD7A");

            entity.Property(e => e.Name)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employees>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0772DE2B0D");

            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");

            entity.HasOne(d => d.IdDepartamentNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdDepartament)
                .HasConstraintName("FK__Employees__IdDep__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
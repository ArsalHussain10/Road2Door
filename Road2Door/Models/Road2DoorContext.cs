using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Road2Door.Models;

public partial class Road2DoorContext : DbContext
{
    public Road2DoorContext()
    {
    }

    public Road2DoorContext(DbContextOptions<Road2DoorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consumer> Consumers { get; set; }

    public virtual DbSet<Rider> Riders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Road2Door;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Consumer__3214EC07788B365E");

            entity.ToTable("Consumer");

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Rider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rider__3214EC076EC9EB2E");

            entity.ToTable("Rider");

            entity.Property(e => e.Cnic)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("cnic");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.License)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("license");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PoliceRecord)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("police_record");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((0))")
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

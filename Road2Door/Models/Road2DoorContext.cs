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

    public virtual DbSet<ConsumerLocation> ConsumerLocations { get; set; }

    public virtual DbSet<InventoryItem> InventoryItems { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<MenuDetail> MenuDetails { get; set; }

    public virtual DbSet<MenueMaster> MenueMasters { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderNotification> OrderNotifications { get; set; }

    public virtual DbSet<Rider> Riders { get; set; }

    public virtual DbSet<RiderLocation> RiderLocations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Road2Door;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Consumer__3214EC078618EC5B");

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

        modelBuilder.Entity<ConsumerLocation>(entity =>
        {
            entity.HasKey(e => e.ConsumerId).HasName("PK__Consumer__B9581C81F85C6F62");

            entity.ToTable("ConsumerLocation");

            entity.Property(e => e.ConsumerId)
                .ValueGeneratedNever()
                .HasColumnName("consumerId");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("longitude");

            entity.HasOne(d => d.Consumer).WithOne(p => p.ConsumerLocation)
                .HasForeignKey<ConsumerLocation>(d => d.ConsumerId)
                .HasConstraintName("FK_ConsumerLocation_ToConsumer");
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Srno).HasName("PK__tmp_ms_x__C3A7DF84C3C713E9");

            entity.ToTable("Inventory_Items");

            entity.HasOne(d => d.Item).WithMany(p => p.InventoryItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_Inventory_Items_ItemId_To_ItemTable");

            entity.HasOne(d => d.Rider).WithMany(p => p.InventoryItems)
                .HasForeignKey(d => d.RiderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Items_RiderId_To_RiderTable");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__56A128AA919E0DCC");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("itemId");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<MenuDetail>(entity =>
        {
            entity.HasKey(e => e.Srno);

            entity.ToTable("Menu_Details");

            entity.HasOne(d => d.Item).WithMany(p => p.MenuDetails)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_Menu_Details_ItemId_To_ItemTable");

            entity.HasOne(d => d.Menue).WithMany(p => p.MenuDetails)
                .HasForeignKey(d => d.MenueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_Details_MenuId_To_MenuMasterTable");
        });

        modelBuilder.Entity<MenueMaster>(entity =>
        {
            entity.HasKey(e => e.MenueId).HasName("PK__Menue_Ma__5C325F0CBE1F1454");

            entity.ToTable("Menue_Master");

            entity.Property(e => e.CreationDate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ExpirationDate)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => new { e.RiderId, e.ConsumerId });

            entity.ToTable("Notification");

            entity.Property(e => e.RiderId).HasColumnName("riderId");
            entity.Property(e => e.ConsumerId).HasColumnName("consumerId");
            entity.Property(e => e.InsertionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("insertionTime");

            entity.HasOne(d => d.Consumer).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ConsumerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Consumer");

            entity.HasOne(d => d.Rider).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.RiderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_ToRider");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF12DE7AD2");

            entity.ToTable("Order");

            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Consumer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ConsumerId)
                .HasConstraintName("FK_Order_ConsumerId_To_ConsumerTable");

            entity.HasOne(d => d.Menu).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_Order_Menu_To_MenueMasterTable");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Sr).HasName("PK__OrderDet__32151FB895EA568B");

            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Item).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ItemId_ToItemss");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderId_ToOrderss");
        });

        modelBuilder.Entity<OrderNotification>(entity =>
        {
            entity.HasKey(e => e.Sr).HasName("PK__OrderNot__32151FB882554452");

            entity.ToTable("OrderNotification");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderNotifications)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderId_Order");

            entity.HasOne(d => d.Rider).WithMany(p => p.OrderNotifications)
                .HasForeignKey(d => d.RiderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RiderId_Rider");
        });

        modelBuilder.Entity<Rider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rider__3214EC07761E4734");

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

        modelBuilder.Entity<RiderLocation>(entity =>
        {
            entity.HasKey(e => e.RiderId).HasName("PK__RiderLoc__DB1C01CD3F72DB54");

            entity.ToTable("RiderLocation");

            entity.Property(e => e.RiderId)
                .ValueGeneratedNever()
                .HasColumnName("riderId");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("longitude");

            entity.HasOne(d => d.Rider).WithOne(p => p.RiderLocation)
                .HasForeignKey<RiderLocation>(d => d.RiderId)
                .HasConstraintName("FK_RiderLocation_ToRider");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

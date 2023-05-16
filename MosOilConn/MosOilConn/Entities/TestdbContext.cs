using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MosOilConn.Entities;

public partial class TestdbContext : DbContext
{
    public TestdbContext()
    {
    }

    public TestdbContext(DbContextOptions<TestdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bonuscard> Bonuscards { get; set; }

    public virtual DbSet<Column> Columns { get; set; }

    public virtual DbSet<FoodBucket> FoodBuckets { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Sysdiagram> Sysdiagrams { get; set; }

    public virtual DbSet<TypeFuel> TypeFuels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGender> UserGenders { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySQL("server=rc1b-65vs4bpq68f1pque.mdb.yandexcloud.net;port=3306;user=SA;password=reallyStrongPwd123;database=testdb;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bonuscard>(entity =>
        {
            entity.HasKey(e => e.IdCard).HasName("PRIMARY");

            entity.ToTable("bonuscard");

            entity.Property(e => e.IdCard).HasColumnName("idCard");
        });

        modelBuilder.Entity<Column>(entity =>
        {
            entity.HasKey(e => e.IdColumn).HasName("PRIMARY");

            entity.ToTable("column");

            entity.HasIndex(e => e.IdStorage, "idStorage");

            entity.Property(e => e.IdColumn).HasColumnName("idColumn");
            entity.Property(e => e.IdStorage).HasColumnName("idStorage");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .HasColumnName("number");

            entity.HasOne(d => d.IdStorageNavigation).WithMany(p => p.Columns)
                .HasForeignKey(d => d.IdStorage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("column_ibfk_1");
        });

        modelBuilder.Entity<FoodBucket>(entity =>
        {
            entity.HasKey(e => e.IdFoodBucket).HasName("PRIMARY");

            entity.ToTable("foodBucket");

            entity.HasIndex(e => e.IdOrder, "idOrder");

            entity.HasIndex(e => e.IdProduct, "idProduct");

            entity.Property(e => e.IdFoodBucket).HasColumnName("idFoodBucket");
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.FoodBuckets)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("foodbucket_ibfk_2");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.FoodBuckets)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("foodbucket_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.IdColumns, "idColumns");

            entity.HasIndex(e => e.IdUser, "idUser");

            entity.HasIndex(e => e.Status, "status");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Data)
                .HasColumnType("date")
                .HasColumnName("data");
            entity.Property(e => e.IdColumns).HasColumnName("idColumns");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

            entity.HasOne(d => d.IdColumnsNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdColumns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_ibfk_1");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_ibfk_3");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_ibfk_4");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PRIMARY");

            entity.ToTable("product");

            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Discription)
                .HasMaxLength(50)
                .HasColumnName("discription");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Photo)
                .HasColumnType("blob")
                .HasColumnName("photo");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.IdProvider).HasName("PRIMARY");

            entity.ToTable("provider");

            entity.Property(e => e.IdProvider).HasColumnName("idProvider");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phoneNumber");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.HasKey(e => e.Status).HasName("PRIMARY");

            entity.ToTable("statusOrder");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.IdStorage).HasName("PRIMARY");

            entity.ToTable("storage");

            entity.HasIndex(e => e.IdProvider, "idProvider");

            entity.HasIndex(e => e.IdTypeFuel, "idTypeFuel");

            entity.Property(e => e.IdStorage).HasColumnName("idStorage");
            entity.Property(e => e.IdProvider).HasColumnName("idProvider");
            entity.Property(e => e.IdTypeFuel).HasColumnName("idTypeFuel");
            entity.Property(e => e.Remainder)
                .HasMaxLength(255)
                .HasColumnName("remainder");

            entity.HasOne(d => d.IdProviderNavigation).WithMany(p => p.Storages)
                .HasForeignKey(d => d.IdProvider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("storage_ibfk_2");

            entity.HasOne(d => d.IdTypeFuelNavigation).WithMany(p => p.Storages)
                .HasForeignKey(d => d.IdTypeFuel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("storage_ibfk_1");
        });

        modelBuilder.Entity<Sysdiagram>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("sysdiagrams");

            entity.Property(e => e.Definition)
                .HasMaxLength(255)
                .HasColumnName("definition");
            entity.Property(e => e.DiagramId)
                .HasMaxLength(255)
                .HasColumnName("diagram_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PrincipalId)
                .HasMaxLength(255)
                .HasColumnName("principal_id");
            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .HasColumnName("version");
        });

        modelBuilder.Entity<TypeFuel>(entity =>
        {
            entity.HasKey(e => e.IdTypeFuel).HasName("PRIMARY");

            entity.ToTable("typeFuel");

            entity.Property(e => e.IdTypeFuel).HasColumnName("idTypeFuel");
            entity.Property(e => e.NameTypeFuel)
                .HasMaxLength(50)
                .HasColumnName("nameTypeFuel");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Gender, "gender");

            entity.HasIndex(e => e.IdCard, "idCard");

            entity.HasIndex(e => e.Role, "role");

            entity.HasIndex(e => e.StatusUser, "statusUser");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Data)
                .HasColumnType("date")
                .HasColumnName("data");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.IdCard).HasColumnName("idCard");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Photo)
                .HasColumnType("blob")
                .HasColumnName("photo");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.StatusUser)
                .HasMaxLength(50)
                .HasColumnName("statusUser");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCard)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.StatusUserNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_4");
        });

        modelBuilder.Entity<UserGender>(entity =>
        {
            entity.HasKey(e => e.Gender).HasName("PRIMARY");

            entity.ToTable("userGenders");

            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Role).HasName("PRIMARY");

            entity.ToTable("userRoles");

            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.HasKey(e => e.StatusUser).HasName("PRIMARY");

            entity.ToTable("userStatus");

            entity.Property(e => e.StatusUser)
                .HasMaxLength(50)
                .HasColumnName("statusUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

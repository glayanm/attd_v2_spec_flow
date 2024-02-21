using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using attd_v2_spec_flow.Entities;

namespace attd_v2_spec_flow.Context
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderLine> OrderLines { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=mysql.tool.net;port=23306;database=db;uid=admin;pwd=123456;charset=utf8;sslmode=None", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.DeliverNo)
                    .HasMaxLength(255)
                    .HasColumnName("deliver_no");

                entity.Property(e => e.DeliveredAt)
                    .HasColumnType("datetime")
                    .HasColumnName("delivered_at");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("product_name");

                entity.Property(e => e.RecipientAddress)
                    .HasMaxLength(255)
                    .HasColumnName("recipient_address");

                entity.Property(e => e.RecipientMobile)
                    .HasMaxLength(255)
                    .HasColumnName("recipient_mobile");

                entity.Property(e => e.RecipientName)
                    .HasMaxLength(255)
                    .HasColumnName("recipient_name");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasColumnName("status");

                entity.Property(e => e.Total)
                    .HasPrecision(19, 2)
                    .HasColumnName("total");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.ToTable("order_lines");

                entity.HasIndex(e => e.OrderId, "FK1smc0s578t2oih21yn9hw6usr");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(255)
                    .HasColumnName("item_name");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasPrecision(19, 2)
                    .HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

﻿// <auto-generated />
using System;
using LaundryOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LaundryOnline.Migrations
{
    [DbContext(typeof(LaundryOnlineContext))]
    [Migration("20221102125640_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LaundryOnline.Models.Banner", b =>
                {
                    b.Property<int>("BannerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BannerTitle")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Image");

                    b.Property<byte>("Status");

                    b.HasKey("BannerId");

                    b.ToTable("Banners");
                });

            modelBuilder.Entity("LaundryOnline.Models.Blog", b =>
                {
                    b.Property<string>("BlogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Image");

                    b.Property<byte>("Status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("UserId");

                    b.HasKey("BlogId");

                    b.HasIndex("UserId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("LaundryOnline.Models.Coupon", b =>
                {
                    b.Property<string>("CouponId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CouponName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<double>("Discount");

                    b.Property<byte>("Status");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("CouponId");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("LaundryOnline.Models.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("ContactNumber")
                        .IsRequired();

                    b.Property<string>("CouponId")
                        .IsRequired();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("Note");

                    b.Property<byte>("OrderStatus");

                    b.Property<string>("PaymentId");

                    b.Property<byte>("PaymentStatus");

                    b.Property<double>("Price");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("OrderId");

                    b.HasIndex("CouponId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("LaundryOnline.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrderId");

                    b.Property<double>("PriceUnit");

                    b.Property<int>("Quantity");

                    b.Property<string>("UnitId");

                    b.HasKey("OrderItemId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UnitId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("LaundryOnline.Models.Payment", b =>
                {
                    b.Property<string>("PaymentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PaymentName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("PaymentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("LaundryOnline.Models.Service", b =>
                {
                    b.Property<string>("ServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ServiceDescription")
                        .IsRequired()
                        .HasColumnType("ntext");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("ServiceId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("LaundryOnline.Models.Unit", b =>
                {
                    b.Property<string>("UnitId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ServiceId");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<double>("UnitPrice");

                    b.HasKey("UnitId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("LaundryOnline.Models.UsedCoupon", b =>
                {
                    b.Property<int>("UsedCouponId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CouponId");

                    b.Property<string>("UserId");

                    b.HasKey("UsedCouponId");

                    b.HasIndex("CouponId");

                    b.HasIndex("UserId");

                    b.ToTable("UsedCoupons");
                });

            modelBuilder.Entity("LaundryOnline.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("ContactNumber")
                        .IsRequired();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<byte>("Status");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LaundryOnline.Models.Blog", b =>
                {
                    b.HasOne("LaundryOnline.Models.User", "User")
                        .WithMany("Blogs")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("LaundryOnline.Models.Order", b =>
                {
                    b.HasOne("LaundryOnline.Models.Coupon", "Coupon")
                        .WithMany("Orders")
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LaundryOnline.Models.Payment", "Payment")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentId");

                    b.HasOne("LaundryOnline.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LaundryOnline.Models.OrderItem", b =>
                {
                    b.HasOne("LaundryOnline.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("LaundryOnline.Models.Unit", "Unit")
                        .WithMany("OrderItems")
                        .HasForeignKey("UnitId");
                });

            modelBuilder.Entity("LaundryOnline.Models.Unit", b =>
                {
                    b.HasOne("LaundryOnline.Models.Service", "Service")
                        .WithMany("Units")
                        .HasForeignKey("ServiceId");
                });

            modelBuilder.Entity("LaundryOnline.Models.UsedCoupon", b =>
                {
                    b.HasOne("LaundryOnline.Models.Coupon", "Coupon")
                        .WithMany("UsedCoupons")
                        .HasForeignKey("CouponId");

                    b.HasOne("LaundryOnline.Models.User", "User")
                        .WithMany("UsedCoupons")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}

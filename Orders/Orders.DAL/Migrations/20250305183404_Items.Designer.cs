﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orders.DAL.Context;

#nullable disable

namespace Orders.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250305183404_Items")]
    partial class Items
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Orders.Domain.Entities.Order", b =>
                {
                    b.Property<long>("Order_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Order_Id"));

                    b.Property<long>("Costumer_Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Order_Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Orders.Domain.Entities.OrderItem", b =>
                {
                    b.Property<long>("OrderItem_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("OrderItem_Id"));

                    b.Property<long>("Order_Id")
                        .HasColumnType("bigint");

                    b.Property<long?>("Order_Id1")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Product_Id")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderItem_Id");

                    b.HasIndex("Order_Id1");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Orders.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("Orders.Domain.Entities.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("Order_Id1");
                });

            modelBuilder.Entity("Orders.Domain.Entities.Order", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

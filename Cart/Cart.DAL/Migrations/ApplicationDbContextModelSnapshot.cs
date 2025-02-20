﻿// <auto-generated />
using System;
using Cart.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cart.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cart.Domain.Domain.Entities.Item", b =>
                {
                    b.Property<long>("Item_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Item_Id"));

                    b.Property<long>("Buyer_Id")
                        .HasColumnType("bigint");

                    b.Property<long>("Cart_Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<long>("Product_Id")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Item_Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Cart.Domain.Domain.Entities.ShoppingCart", b =>
                {
                    b.Property<long>("Cart_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Cart_Id"));

                    b.Property<long>("Buyer_Id")
                        .HasColumnType("bigint");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<long>("User_Id")
                        .HasColumnType("bigint");

                    b.HasKey("Cart_Id");

                    b.ToTable("Carts");
                });
#pragma warning restore 612, 618
        }
    }
}

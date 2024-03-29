﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestaurantSimulation.Infrastructure.Persistence;

#nullable disable

namespace RestaurantSimulation.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(RestaurantSimulationContext))]
    partial class RestaurantSimulationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RestaurantSimulation.Domain.Entities.Authentication.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("Sub")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("RestaurantSimulation.Domain.Entities.Restaurant.MenuCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("MenuCategory", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("694d6ed1-4ef5-4539-926d-c459c2ba1b39"),
                            Description = "Here you will have all the dishes you can eat in the morning.",
                            Name = "Breakfast"
                        },
                        new
                        {
                            Id = new Guid("d63d9f83-eb6a-4aa0-b7e5-64370978c8c1"),
                            Description = "Here you will find among the best salads.",
                            Name = "Salads"
                        },
                        new
                        {
                            Id = new Guid("7dd4b57d-3601-43d6-bf7a-86710f613d45"),
                            Description = "Quality seafood and fish dishes.",
                            Name = "Seafood"
                        },
                        new
                        {
                            Id = new Guid("503595d7-3306-4f09-b1ab-73beb764cf93"),
                            Description = "Life is short, eat the dessert.",
                            Name = "Desserts"
                        },
                        new
                        {
                            Id = new Guid("ee920cbc-bcef-4ac5-9630-3c87321e5b76"),
                            Description = "Hot and Cold drinks for everyone.",
                            Name = "Drinks"
                        },
                        new
                        {
                            Id = new Guid("f63587f5-0da9-4517-80e5-2e3035f0ba19"),
                            Description = "Here you will find all kinds of pizza.",
                            Name = "Pizzas"
                        },
                        new
                        {
                            Id = new Guid("02c63d08-fc05-493e-add6-00e56eb74686"),
                            Description = "Here you will find dishes cooked on the grill.",
                            Name = "Grill "
                        });
                });

            modelBuilder.Entity("RestaurantSimulation.Domain.Entities.Restaurant.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("RestaurantSimulation.Domain.Entities.Restaurant.Product", b =>
                {
                    b.HasOne("RestaurantSimulation.Domain.Entities.Restaurant.MenuCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("RestaurantSimulation.Domain.Entities.Restaurant.MenuCategory", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

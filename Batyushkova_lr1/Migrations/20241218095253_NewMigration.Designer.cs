﻿// <auto-generated />
using System;
using Batyushkova_lr1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Batyushkova_lr1.Migrations
{
    [DbContext(typeof(Batyushkova_lr1Context))]
    [Migration("20241218095253_NewMigration")]
    partial class NewMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("Batyushkova_lr1.Models.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Dish");
                });

            modelBuilder.Entity("Batyushkova_lr1.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TableId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeOrdered")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TableId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Batyushkova_lr1.Models.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsOccupied")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Table");
                });

            modelBuilder.Entity("Batyushkova_lr1.Models.Dish", b =>
                {
                    b.HasOne("Batyushkova_lr1.Models.Order", null)
                        .WithMany("Dishes")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("Batyushkova_lr1.Models.Order", b =>
                {
                    b.HasOne("Batyushkova_lr1.Models.Table", "Table")
                        .WithMany()
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Table");
                });

            modelBuilder.Entity("Batyushkova_lr1.Models.Order", b =>
                {
                    b.Navigation("Dishes");
                });
#pragma warning restore 612, 618
        }
    }
}

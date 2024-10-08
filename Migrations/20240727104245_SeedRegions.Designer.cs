﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NzWalks.Data;

#nullable disable

namespace NzWalks.Migrations
{
    [DbContext(typeof(NzWalksDbContext))]
    [Migration("20240727104245_SeedRegions")]
    partial class SeedRegions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NzWalks.Models.Domain.Difficulty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Difficulties");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f362b16a-6f46-4f16-a01a-ae7a9b8c2678"),
                            Name = "Easy"
                        },
                        new
                        {
                            Id = new Guid("b084bc9e-da20-43c7-831f-56fa43eab106"),
                            Name = "Medium"
                        },
                        new
                        {
                            Id = new Guid("6c218143-7a91-47bc-9c17-d9cd7a92d9c7"),
                            Name = "Hard"
                        });
                });

            modelBuilder.Entity("NzWalks.Models.Domain.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegionImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Regions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("919276a6-b956-41f5-856c-f27653292008"),
                            Code = "AKL",
                            Name = "New Zealand",
                            RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9e/Auckland_City_Skyline.jpg/1200px-Auckland_City_Skyline.jpg"
                        },
                        new
                        {
                            Id = new Guid("919276a6-b956-41f5-856c-f27653292009"),
                            Code = "QLD",
                            Name = "Queensland",
                            RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Brisbane_CITY_Harbour_and_CBD.jpg/1200px-Brisbane_CITY_Harbour_and_CBD.jpg"
                        },
                        new
                        {
                            Id = new Guid("919276a6-b956-41f5-856c-f27653292010"),
                            Code = "VIC",
                            Name = "Victoria",
                            RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/01/Melbourne_City_Skyline.jpg/1200px-Melbourne_City_Skyline.jpg"
                        },
                        new
                        {
                            Id = new Guid("919276a6-b956-41f5-856c-f27653292011"),
                            Code = "TAS",
                            Name = "Tasmania",
                            RegionImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/Hobart_City_Skyline.jpg/1200px-Hobart_City_Skyline.jpg"
                        });
                });

            modelBuilder.Entity("NzWalks.Models.Domain.Walk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DifficultyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("LengthInKm")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("WalkImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DifficultyId");

                    b.HasIndex("RegionId");

                    b.ToTable("Walks");
                });

            modelBuilder.Entity("NzWalks.Models.Domain.Walk", b =>
                {
                    b.HasOne("NzWalks.Models.Domain.Difficulty", "Difficulty")
                        .WithMany()
                        .HasForeignKey("DifficultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NzWalks.Models.Domain.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Difficulty");

                    b.Navigation("Region");
                });
#pragma warning restore 612, 618
        }
    }
}

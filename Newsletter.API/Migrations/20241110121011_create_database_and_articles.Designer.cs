﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newsletter.API.Database;

#nullable disable

namespace Newsletter.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241110121011_create_database_and_articles")]
    partial class create_database_and_articles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Newsletter.API.Entities.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PublishedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Newsletter.API.Entities.Article", b =>
                {
                    b.OwnsOne("System.Collections.Generic.List<string>", "Tags", b1 =>
                        {
                            b1.Property<Guid>("ArticleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Capacity")
                                .HasColumnType("int");

                            b1.HasKey("ArticleId");

                            b1.ToTable("Articles");

                            b1.ToJson("Tags");

                            b1.WithOwner()
                                .HasForeignKey("ArticleId");
                        });

                    b.Navigation("Tags")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

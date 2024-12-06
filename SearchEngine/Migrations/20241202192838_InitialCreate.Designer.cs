﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SearchEngine.Data;

#nullable disable

namespace SearchEngine.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241202192838_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SearchEngine.Models.SearchRecord", b =>
                {
                    b.Property<int>("SearchRecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SearchRecordId"));

                    b.Property<DateTime>("EnteredDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SearchEngine")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("SearchRecordId");

                    b.ToTable("SearchRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
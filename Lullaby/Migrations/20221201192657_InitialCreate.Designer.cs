﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lullaby.Migrations
{
    using Db;

    [DbContext(typeof(LullabyContext))]
    [Migration("20221201192657_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Lullaby.Models.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("EventDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("EventEnds")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EventPlace")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("EventStarts")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("GroupKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDateTimeDetailed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });
#pragma warning restore 612, 618
        }
    }
}

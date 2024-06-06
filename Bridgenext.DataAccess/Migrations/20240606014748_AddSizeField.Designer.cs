﻿// <auto-generated />
using System;
using Bridgenext.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bridgenext.DataAccess.Migrations
{
    [DbContext(typeof(UserSystemContext))]
    [Migration("20240606014748_AddSizeField")]
    partial class AddSizeField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bridgenext.Models.Schema.Addreesses", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("date");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid");

                    b.Property<string>("Line1")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Line2")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("date");

                    b.Property<string>("ModifyUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("City");

                    b.HasIndex("Country");

                    b.HasIndex("IdUser");

                    b.HasIndex("Zip");

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Addreesses_History", b =>
                {
                    b.Property<Guid>("AuditId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuditAction")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("AuditDate")
                        .HasColumnType("date");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("date");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid");

                    b.Property<string>("Line1")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Line2")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("date");

                    b.Property<string>("ModifyUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("AuditId");

                    b.ToTable("Addresses_History", (string)null);
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Comments", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("IdDocumnet")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("IdDocumnet");

                    b.HasIndex("IdUser");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Documents", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Context")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("date");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<bool>("Hide")
                        .HasColumnType("boolean");

                    b.Property<int>("IdDocumentType")
                        .HasColumnType("integer");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("date");

                    b.Property<string>("ModifyUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("SourceFile")
                        .HasColumnType("text");

                    b.Property<string>("TargetFile")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdDocumentType");

                    b.HasIndex("IdUser");

                    b.ToTable("Document", (string)null);
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.DocumentsType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DocumentType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Text"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Doc"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Image"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Video"
                        });
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Documents_History", b =>
                {
                    b.Property<Guid>("AuditId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuditAction")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("AuditDate")
                        .HasColumnType("date");

                    b.Property<string>("Context")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("date");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<bool>("Hide")
                        .HasColumnType("boolean");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("IdDocumentType")
                        .HasColumnType("integer");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("date");

                    b.Property<string>("ModifyUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("SourceFile")
                        .HasColumnType("text");

                    b.Property<string>("TargetFile")
                        .HasColumnType("text");

                    b.HasKey("AuditId");

                    b.ToTable("Document_History", (string)null);
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("date");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("IdUserType")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("date");

                    b.Property<string>("ModifyUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IdUserType");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("679bd613-da71-48b9-bf5c-b7b598935b77"),
                            CreateDate = new DateTime(2024, 6, 5, 22, 47, 47, 393, DateTimeKind.Utc).AddTicks(2583),
                            CreateUser = "Administrator",
                            Email = "admin@admin.admin",
                            FirstName = "Administrator",
                            IdUserType = 1,
                            LastName = "Administrator",
                            ModifyDate = new DateTime(2024, 6, 5, 22, 47, 47, 393, DateTimeKind.Utc).AddTicks(2593),
                            ModifyUser = "Administrator"
                        });
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.UsersTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UsersType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            Type = "AMBContext"
                        });
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Users_History", b =>
                {
                    b.Property<Guid>("AuditId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuditAction")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("AuditDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("date");

                    b.Property<string>("CreateUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("IdUserType")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("date");

                    b.Property<string>("ModifyUser")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("AuditId");

                    b.ToTable("Users_History", (string)null);
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Addreesses", b =>
                {
                    b.HasOne("Bridgenext.Models.Schema.Users", "User")
                        .WithMany("Addreesses")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Comments", b =>
                {
                    b.HasOne("Bridgenext.Models.Schema.Documents", "Documents")
                        .WithMany("Comments")
                        .HasForeignKey("IdDocumnet")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bridgenext.Models.Schema.Users", "Users")
                        .WithMany("Comments")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Documents");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Documents", b =>
                {
                    b.HasOne("Bridgenext.Models.Schema.DocumentsType", "DocumentType")
                        .WithMany("Documents")
                        .HasForeignKey("IdDocumentType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bridgenext.Models.Schema.Users", "Users")
                        .WithMany("Documents")
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocumentType");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Users", b =>
                {
                    b.HasOne("Bridgenext.Models.Schema.UsersTypes", "UserTypes")
                        .WithMany("Users")
                        .HasForeignKey("IdUserType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserTypes");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Documents", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.DocumentsType", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.Users", b =>
                {
                    b.Navigation("Addreesses");

                    b.Navigation("Comments");

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("Bridgenext.Models.Schema.UsersTypes", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}

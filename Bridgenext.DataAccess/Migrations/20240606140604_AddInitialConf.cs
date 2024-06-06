using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bridgenext.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses_History",
                columns: table => new
                {
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    Line1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Line2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreateUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    ModifyUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditAction = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AuditDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses_History", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "Document_History",
                columns: table => new
                {
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    SourceFile = table.Column<string>(type: "text", nullable: true),
                    TargetFile = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    Hide = table.Column<bool>(type: "boolean", nullable: false),
                    IdDocumentType = table.Column<int>(type: "integer", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    MongoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    ModifyUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditAction = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AuditDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document_History", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users_History",
                columns: table => new
                {
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    IdUserType = table.Column<int>(type: "integer", nullable: false),
                    CreateUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    ModifyUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "date", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditAction = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AuditDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_History", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "UsersType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    IdUserType = table.Column<int>(type: "integer", nullable: false),
                    CreateUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    ModifyUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UsersType_IdUserType",
                        column: x => x.IdUserType,
                        principalTable: "UsersType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    Line1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Line2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Zip = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreateUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    ModifyUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    SourceFile = table.Column<string>(type: "text", nullable: true),
                    TargetFile = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    Hide = table.Column<bool>(type: "boolean", nullable: false),
                    IdDocumentType = table.Column<int>(type: "integer", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    MongoId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false),
                    ModifyUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_DocumentType_IdDocumentType",
                        column: x => x.IdDocumentType,
                        principalTable: "DocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false),
                    IdDocumnet = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Document_IdDocumnet",
                        column: x => x.IdDocumnet,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DocumentType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Text" },
                    { 2, "Doc" },
                    { 3, "Image" },
                    { 4, "Video" }
                });

            migrationBuilder.InsertData(
                table: "UsersType",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Administrator" },
                    { 2, "AMBContext" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreateDate", "CreateUser", "Email", "FirstName", "IdUserType", "LastName", "ModifyDate", "ModifyUser" },
                values: new object[] { new Guid("679bd613-da71-48b9-bf5c-b7b598935b77"), new DateTime(2024, 6, 6, 11, 6, 3, 511, DateTimeKind.Utc).AddTicks(6294), "Administrator", "admin@admin.admin", "Administrator", 1, "Administrator", new DateTime(2024, 6, 6, 11, 6, 3, 511, DateTimeKind.Utc).AddTicks(6306), "Administrator" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City",
                table: "Addresses",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Country",
                table: "Addresses",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_IdUser",
                table: "Addresses",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Zip",
                table: "Addresses",
                column: "Zip");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdDocumnet",
                table: "Comments",
                column: "IdDocumnet");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IdUser",
                table: "Comments",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Document_IdDocumentType",
                table: "Document",
                column: "IdDocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_Document_IdUser",
                table: "Document",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdUserType",
                table: "Users",
                column: "IdUserType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Addresses_History");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Document_History");

            migrationBuilder.DropTable(
                name: "Users_History");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UsersType");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElasticSentinel.Migrations
{
    /// <inheritdoc />
    public partial class Phase4Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "documents_processing_detail",
                columns: table => new
                {
                    documents_processing_detail_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_dttm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    document_data = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<char>(type: "TEXT", nullable: false),
                    is_notified = table.Column<bool>(type: "INTEGER", nullable: false),
                    retry_attempts = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents_processing_detail", x => x.documents_processing_detail_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documents_processing_detail");
        }
    }
}

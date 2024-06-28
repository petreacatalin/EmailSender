using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailSender.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignCommands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignCount = table.Column<int>(type: "int", nullable: false),
                    EmailLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCommands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailCommands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommandNumberOfEmails = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EmailCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailCommands_CampaignCommands_CampaignCommandId",
                        column: x => x.CampaignCommandId,
                        principalTable: "CampaignCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailCommands_EmailCounts_EmailCountId",
                        column: x => x.EmailCountId,
                        principalTable: "EmailCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailCommandLogMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CampaignCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLogs_CampaignCommands_CampaignCommandId",
                        column: x => x.CampaignCommandId,
                        principalTable: "CampaignCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailLogs_EmailCommands_EmailCommandId",
                        column: x => x.EmailCommandId,
                        principalTable: "EmailCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailCommands_CampaignCommandId",
                table: "EmailCommands",
                column: "CampaignCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailCommands_EmailCountId",
                table: "EmailCommands",
                column: "EmailCountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_CampaignCommandId",
                table: "EmailLogs",
                column: "CampaignCommandId",
                unique: true,
                filter: "[CampaignCommandId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_EmailCommandId",
                table: "EmailLogs",
                column: "EmailCommandId",
                unique: true,
                filter: "[EmailCommandId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "EmailCommands");

            migrationBuilder.DropTable(
                name: "CampaignCommands");

            migrationBuilder.DropTable(
                name: "EmailCounts");
        }
    }
}

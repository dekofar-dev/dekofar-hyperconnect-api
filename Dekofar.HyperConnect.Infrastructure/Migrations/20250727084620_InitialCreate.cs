using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dekofar.HyperConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_AspNetUsers_AssignedToUserId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketNotes_AspNetUsers_CreatedByUserId",
                table: "TicketNotes");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_AssignedToUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TicketNotes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "TicketNotes",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "TicketNotes",
                newName: "TicketId1");

            migrationBuilder.RenameIndex(
                name: "IX_TicketNotes_CreatedByUserId",
                table: "TicketNotes",
                newName: "IX_TicketNotes_TicketId1");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TicketNotes",
                type: "uuid",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "SupportTickets",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "SupportTickets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "SupportTickets",
                type: "uuid",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "SupportTickets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhone",
                table: "SupportTickets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "SupportTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SupportTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "SupportTickets",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: false),
                    TicketId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketLogs_SupportTickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "SupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketLogs_SupportTickets_TicketId1",
                        column: x => x.TicketId1,
                        principalTable: "SupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketLogs_TicketId",
                table: "TicketLogs",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketLogs_TicketId1",
                table: "TicketLogs",
                column: "TicketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketNotes_SupportTickets_TicketId1",
                table: "TicketNotes",
                column: "TicketId1",
                principalTable: "SupportTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketNotes_SupportTickets_TicketId1",
                table: "TicketNotes");

            migrationBuilder.DropTable(
                name: "TicketLogs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TicketNotes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "CustomerPhone",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "SupportTickets");

            migrationBuilder.RenameColumn(
                name: "TicketId1",
                table: "TicketNotes",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "TicketNotes",
                newName: "Note");

            migrationBuilder.RenameIndex(
                name: "IX_TicketNotes_TicketId1",
                table: "TicketNotes",
                newName: "IX_TicketNotes_CreatedByUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TicketNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "SupportTickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedToUserId",
                table: "SupportTickets",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SupportTickets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_AssignedToUserId",
                table: "SupportTickets",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_AspNetUsers_AssignedToUserId",
                table: "SupportTickets",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketNotes_AspNetUsers_CreatedByUserId",
                table: "TicketNotes",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

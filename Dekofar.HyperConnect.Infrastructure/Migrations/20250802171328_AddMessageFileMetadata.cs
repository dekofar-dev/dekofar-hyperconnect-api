using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dekofar.HyperConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageFileMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttachmentUrl",
                table: "UserMessages",
                newName: "FileUrl");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "UserMessages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "UserMessages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "UserMessages");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "UserMessages",
                newName: "AttachmentUrl");
        }
    }
}

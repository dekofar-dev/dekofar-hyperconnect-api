using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dekofar.HyperConnect.Infrastructure.Persistence.Migrations
{
    public partial class SyncEntitiesWithDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"AspNetUsers\" ADD COLUMN IF NOT EXISTS \"FullName\" text;");
            migrationBuilder.Sql("ALTER TABLE \"AspNetUsers\" ADD COLUMN IF NOT EXISTS \"AvatarUrl\" text;");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" ADD COLUMN IF NOT EXISTS \"Priority\" integer NOT NULL DEFAULT 1;");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" ADD COLUMN IF NOT EXISTS \"DueDate\" timestamp with time zone;");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" ADD COLUMN IF NOT EXISTS \"FilePath\" text;");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" ADD COLUMN IF NOT EXISTS \"LastUpdatedAt\" timestamp with time zone NOT NULL DEFAULT NOW();");
            migrationBuilder.Sql("ALTER TABLE \"SupportCategories\" ADD COLUMN IF NOT EXISTS \"Description\" character varying(250);");
            migrationBuilder.Sql("ALTER TABLE \"SupportCategories\" ADD COLUMN IF NOT EXISTS \"CreatedAt\" timestamp with time zone NOT NULL DEFAULT NOW();");
            migrationBuilder.Sql("ALTER TABLE \"Permissions\" ADD COLUMN IF NOT EXISTS \"Description\" character varying(250);");

            migrationBuilder.Sql("INSERT INTO \"SupportCategories\" (\"Id\", \"Name\", \"Description\", \"CreatedAt\") VALUES ('11111111-1111-1111-1111-111111111111', 'Technical', 'Issues related to technical problems', '2024-01-01T00:00:00Z') ON CONFLICT (\"Id\") DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"SupportCategoryRoles\" (\"Id\", \"SupportCategoryId\", \"RoleName\") VALUES ('44444444-4444-4444-4444-444444444444', '11111111-1111-1111-1111-111111111111', 'Admin') ON CONFLICT (\"Id\") DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"Permissions\" (\"Id\", \"Name\", \"Description\") VALUES ('22222222-2222-2222-2222-222222222222', 'SupportTickets.Manage', 'Manage support tickets') ON CONFLICT (\"Id\") DO NOTHING;");
            migrationBuilder.Sql("INSERT INTO \"RolePermissions\" (\"Id\", \"RoleName\", \"PermissionId\") VALUES ('33333333-3333-3333-3333-333333333333', 'Admin', '22222222-2222-2222-2222-222222222222') ON CONFLICT (\"Id\") DO NOTHING;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"RolePermissions\" WHERE \"Id\"='33333333-3333-3333-3333-333333333333';");
            migrationBuilder.Sql("DELETE FROM \"Permissions\" WHERE \"Id\"='22222222-2222-2222-2222-222222222222';");
            migrationBuilder.Sql("DELETE FROM \"SupportCategoryRoles\" WHERE \"Id\"='44444444-4444-4444-4444-444444444444';");
            migrationBuilder.Sql("DELETE FROM \"SupportCategories\" WHERE \"Id\"='11111111-1111-1111-1111-111111111111';");

            migrationBuilder.Sql("ALTER TABLE \"AspNetUsers\" DROP COLUMN IF EXISTS \"FullName\";");
            migrationBuilder.Sql("ALTER TABLE \"AspNetUsers\" DROP COLUMN IF EXISTS \"AvatarUrl\";");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" DROP COLUMN IF EXISTS \"Priority\";");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" DROP COLUMN IF EXISTS \"DueDate\";");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" DROP COLUMN IF EXISTS \"FilePath\";");
            migrationBuilder.Sql("ALTER TABLE \"SupportTickets\" DROP COLUMN IF EXISTS \"LastUpdatedAt\";");
            migrationBuilder.Sql("ALTER TABLE \"SupportCategories\" DROP COLUMN IF EXISTS \"Description\";");
            migrationBuilder.Sql("ALTER TABLE \"SupportCategories\" DROP COLUMN IF EXISTS \"CreatedAt\";");
            migrationBuilder.Sql("ALTER TABLE \"Permissions\" DROP COLUMN IF EXISTS \"Description\";");
        }
    }
}

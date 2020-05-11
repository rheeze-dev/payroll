using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Attendance4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Editor",
                table: "Attendance",
                newName: "EditorTimeOut");

            migrationBuilder.AddColumn<string>(
                name: "EditorTimeIn",
                table: "Attendance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditorTimeIn",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "EditorTimeOut",
                table: "Attendance",
                newName: "Editor");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustomer",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsSupportEngineer",
                table: "AspNetUsers",
                newName: "IsStudent");

            migrationBuilder.RenameColumn(
                name: "IsSupportAgent",
                table: "AspNetUsers",
                newName: "IsFaculty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsStudent",
                table: "AspNetUsers",
                newName: "IsSupportEngineer");

            migrationBuilder.RenameColumn(
                name: "IsFaculty",
                table: "AspNetUsers",
                newName: "IsSupportAgent");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomer",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}

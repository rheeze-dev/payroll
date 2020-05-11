using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class DbUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Customer_customerId",
                table: "Contact");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "SupportAgent");

            migrationBuilder.DropTable(
                name: "SupportEngineer");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Contact_customerId",
                table: "Contact");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeCreated",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateTimeCreated",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customerId = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    address = table.Column<string>(maxLength: 100, nullable: true),
                    customerName = table.Column<string>(maxLength: 100, nullable: false),
                    customerType = table.Column<int>(nullable: false),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    linkedin = table.Column<string>(maxLength: 100, nullable: true),
                    organizationId = table.Column<Guid>(nullable: false),
                    phone = table.Column<string>(maxLength: 20, nullable: true),
                    thumbUrl = table.Column<string>(maxLength: 255, nullable: true),
                    website = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customerId);
                    table.ForeignKey(
                        name: "FK_Customer_Organization_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organization",
                        principalColumn: "organizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    productId = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    organizationId = table.Column<Guid>(nullable: false),
                    productCategory = table.Column<int>(nullable: false),
                    productName = table.Column<string>(maxLength: 100, nullable: false),
                    thumbUrl = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.productId);
                    table.ForeignKey(
                        name: "FK_Product_Organization_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organization",
                        principalColumn: "organizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportAgent",
                columns: table => new
                {
                    supportAgentId = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    applicationUserId = table.Column<string>(nullable: true),
                    organizationId = table.Column<Guid>(nullable: false),
                    supportAgentName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportAgent", x => x.supportAgentId);
                    table.ForeignKey(
                        name: "FK_SupportAgent_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportAgent_Organization_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organization",
                        principalColumn: "organizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportEngineer",
                columns: table => new
                {
                    supportEngineerId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    applicationUserId = table.Column<string>(nullable: true),
                    organizationId = table.Column<Guid>(nullable: false),
                    supportEngineerName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportEngineer", x => x.supportEngineerId);
                    table.ForeignKey(
                        name: "FK_SupportEngineer_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportEngineer_Organization_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organization",
                        principalColumn: "organizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ticketId = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    contactId = table.Column<Guid>(nullable: false),
                    customerId = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(maxLength: 200, nullable: false),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    organizationId = table.Column<Guid>(nullable: false),
                    phone = table.Column<string>(maxLength: 20, nullable: true),
                    productId = table.Column<Guid>(nullable: false),
                    supportAgentId = table.Column<Guid>(nullable: false),
                    supportEngineerId = table.Column<Guid>(nullable: false),
                    ticketChannel = table.Column<int>(nullable: false),
                    ticketName = table.Column<string>(maxLength: 100, nullable: false),
                    ticketPriority = table.Column<int>(nullable: false),
                    ticketStatus = table.Column<int>(nullable: false),
                    ticketType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ticketId);
                    table.ForeignKey(
                        name: "FK_Ticket_Organization_organizationId",
                        column: x => x.organizationId,
                        principalTable: "Organization",
                        principalColumn: "organizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_customerId",
                table: "Contact",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_organizationId",
                table: "Customer",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_organizationId",
                table: "Product",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportAgent_applicationUserId",
                table: "SupportAgent",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportAgent_organizationId",
                table: "SupportAgent",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportEngineer_applicationUserId",
                table: "SupportEngineer",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportEngineer_organizationId",
                table: "SupportEngineer",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_organizationId",
                table: "Ticket",
                column: "organizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Customer_customerId",
                table: "Contact",
                column: "customerId",
                principalTable: "Customer",
                principalColumn: "customerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

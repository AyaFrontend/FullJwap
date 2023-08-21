using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jwap.DAL.Migrations
{
    public partial class mmiiggghj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InCall",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CallOffers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CallerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CalleeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CallDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CallState = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallOffers_AspNetUsers_CalleeId",
                        column: x => x.CalleeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CallOffers_AspNetUsers_CallerId",
                        column: x => x.CallerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CallOffers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallOffers_CalleeId",
                table: "CallOffers",
                column: "CalleeId");

            migrationBuilder.CreateIndex(
                name: "IX_CallOffers_CallerId",
                table: "CallOffers",
                column: "CallerId");

            migrationBuilder.CreateIndex(
                name: "IX_CallOffers_UserId",
                table: "CallOffers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallOffers");

            migrationBuilder.DropColumn(
                name: "InCall",
                table: "AspNetUsers");
        }
    }
}

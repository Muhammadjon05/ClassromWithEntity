using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class AddJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SendRequestToJoins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromWhomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsJoined = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendRequestToJoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendRequestToJoins_Sciences_ScienceId",
                        column: x => x.ScienceId,
                        principalTable: "Sciences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SendRequestToJoins_users_FromWhomId",
                        column: x => x.FromWhomId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SendRequestToJoins_FromWhomId",
                table: "SendRequestToJoins",
                column: "FromWhomId");

            migrationBuilder.CreateIndex(
                name: "IX_SendRequestToJoins_ScienceId",
                table: "SendRequestToJoins",
                column: "ScienceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SendRequestToJoins");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeightLossTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoalId",
                table: "WeightEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeightEntries_GoalId",
                table: "WeightEntries",
                column: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightEntries_Goals_GoalId",
                table: "WeightEntries",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightEntries_Goals_GoalId",
                table: "WeightEntries");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_WeightEntries_GoalId",
                table: "WeightEntries");

            migrationBuilder.DropColumn(
                name: "GoalId",
                table: "WeightEntries");
        }
    }
}

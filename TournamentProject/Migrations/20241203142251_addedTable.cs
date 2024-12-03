using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentProject.Migrations
{
    /// <inheritdoc />
    public partial class addedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comfirm",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comfirm", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comfirm");
        }
    }
}

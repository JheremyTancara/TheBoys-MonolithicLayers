using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actor_Movies_MovieID",
                table: "Actor");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Director_DirectorID",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Director",
                table: "Director");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actor",
                table: "Actor");

            migrationBuilder.RenameTable(
                name: "Director",
                newName: "Directors");

            migrationBuilder.RenameTable(
                name: "Actor",
                newName: "Actors");

            migrationBuilder.RenameIndex(
                name: "IX_Actor_MovieID",
                table: "Actors",
                newName: "IX_Actors_MovieID");

            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Movies",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Directors",
                table: "Directors",
                column: "DirectorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actors",
                table: "Actors",
                column: "ActorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actors_Movies_MovieID",
                table: "Actors",
                column: "MovieID",
                principalTable: "Movies",
                principalColumn: "MovieID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Directors_DirectorID",
                table: "Movies",
                column: "DirectorID",
                principalTable: "Directors",
                principalColumn: "DirectorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actors_Movies_MovieID",
                table: "Actors");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Directors_DirectorID",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Directors",
                table: "Directors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actors",
                table: "Actors");

            migrationBuilder.RenameTable(
                name: "Directors",
                newName: "Director");

            migrationBuilder.RenameTable(
                name: "Actors",
                newName: "Actor");

            migrationBuilder.RenameIndex(
                name: "IX_Actors_MovieID",
                table: "Actor",
                newName: "IX_Actor_MovieID");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Movies",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Director",
                table: "Director",
                column: "DirectorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actor",
                table: "Actor",
                column: "ActorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Actor_Movies_MovieID",
                table: "Actor",
                column: "MovieID",
                principalTable: "Movies",
                principalColumn: "MovieID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Director_DirectorID",
                table: "Movies",
                column: "DirectorID",
                principalTable: "Director",
                principalColumn: "DirectorID");
        }
    }
}

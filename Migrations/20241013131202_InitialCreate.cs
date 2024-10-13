﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    DirectorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumberOfAwards = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilePictureUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.DirectorID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SubscriptionLevel = table.Column<int>(type: "int", nullable: false),
                    ProfilePicture = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Duration = table.Column<double>(type: "double", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DirectorID = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrailerUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieID);
                    table.ForeignKey(
                        name: "FK_Movies_Directors_DirectorID",
                        column: x => x.DirectorID,
                        principalTable: "Directors",
                        principalColumn: "DirectorID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Movies = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MovieID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilePictureUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorID);
                    table.ForeignKey(
                        name: "FK_Actors_Movies_MovieID",
                        column: x => x.MovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRecommendedMovies",
                columns: table => new
                {
                    RecommendedMoviesMovieID = table.Column<int>(type: "int", nullable: false),
                    User1UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecommendedMovies", x => new { x.RecommendedMoviesMovieID, x.User1UserID });
                    table.ForeignKey(
                        name: "FK_UserRecommendedMovies_Movies_RecommendedMoviesMovieID",
                        column: x => x.RecommendedMoviesMovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRecommendedMovies_Users_User1UserID",
                        column: x => x.User1UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserWatchedMovies",
                columns: table => new
                {
                    User2UserID = table.Column<int>(type: "int", nullable: false),
                    WatchedMoviesMovieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWatchedMovies", x => new { x.User2UserID, x.WatchedMoviesMovieID });
                    table.ForeignKey(
                        name: "FK_UserWatchedMovies_Movies_WatchedMoviesMovieID",
                        column: x => x.WatchedMoviesMovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWatchedMovies_Users_User2UserID",
                        column: x => x.User2UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserWatchlist",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    WatchlistMovieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWatchlist", x => new { x.UserID, x.WatchlistMovieID });
                    table.ForeignKey(
                        name: "FK_UserWatchlist_Movies_WatchlistMovieID",
                        column: x => x.WatchlistMovieID,
                        principalTable: "Movies",
                        principalColumn: "MovieID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWatchlist_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_MovieID",
                table: "Actors",
                column: "MovieID");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_DirectorID",
                table: "Movies",
                column: "DirectorID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecommendedMovies_User1UserID",
                table: "UserRecommendedMovies",
                column: "User1UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchedMovies_WatchedMoviesMovieID",
                table: "UserWatchedMovies",
                column: "WatchedMoviesMovieID");

            migrationBuilder.CreateIndex(
                name: "IX_UserWatchlist_WatchlistMovieID",
                table: "UserWatchlist",
                column: "WatchlistMovieID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "UserRecommendedMovies");

            migrationBuilder.DropTable(
                name: "UserWatchedMovies");

            migrationBuilder.DropTable(
                name: "UserWatchlist");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Directors");
        }
    }
}
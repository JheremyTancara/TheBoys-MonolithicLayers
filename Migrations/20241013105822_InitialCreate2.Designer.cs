﻿using System;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241013105822_InitialCreate2")]
    partial class InitialCreate2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Api.Models.Actor", b =>
                {
                    b.Property<int>("ActorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ActorID"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("MovieID")
                        .HasColumnType("int");

                    b.Property<string>("Movies")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ActorID");

                    b.HasIndex("MovieID");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("Api.Models.Director", b =>
                {
                    b.Property<int>("DirectorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DirectorID"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("NumberOfAwards")
                        .HasColumnType("int");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DirectorID");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("Api.Models.Movie", b =>
                {
                    b.Property<int>("MovieID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("MovieID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("DirectorID")
                        .HasColumnType("int");

                    b.Property<double>("Duration")
                        .HasColumnType("double");

                    b.Property<int>("Genre")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TrailerUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("MovieID");

                    b.HasIndex("DirectorID");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Api.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserID"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ProfilePicture")
                        .HasColumnType("int");

                    b.Property<int>("SubscriptionLevel")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MovieUser", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("WatchlistMovieID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "WatchlistMovieID");

                    b.HasIndex("WatchlistMovieID");

                    b.ToTable("UserWatchlist", (string)null);
                });

            modelBuilder.Entity("MovieUser1", b =>
                {
                    b.Property<int>("RecommendedMoviesMovieID")
                        .HasColumnType("int");

                    b.Property<int>("User1UserID")
                        .HasColumnType("int");

                    b.HasKey("RecommendedMoviesMovieID", "User1UserID");

                    b.HasIndex("User1UserID");

                    b.ToTable("UserRecommendedMovies", (string)null);
                });

            modelBuilder.Entity("MovieUser2", b =>
                {
                    b.Property<int>("User2UserID")
                        .HasColumnType("int");

                    b.Property<int>("WatchedMoviesMovieID")
                        .HasColumnType("int");

                    b.HasKey("User2UserID", "WatchedMoviesMovieID");

                    b.HasIndex("WatchedMoviesMovieID");

                    b.ToTable("UserWatchedMovies", (string)null);
                });

            modelBuilder.Entity("Api.Models.Actor", b =>
                {
                    b.HasOne("Api.Models.Movie", null)
                        .WithMany("Cast")
                        .HasForeignKey("MovieID");
                });

            modelBuilder.Entity("Api.Models.Movie", b =>
                {
                    b.HasOne("Api.Models.Director", "Director")
                        .WithMany()
                        .HasForeignKey("DirectorID");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("MovieUser", b =>
                {
                    b.HasOne("Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("WatchlistMovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieUser1", b =>
                {
                    b.HasOne("Api.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("RecommendedMoviesMovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("User1UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieUser2", b =>
                {
                    b.HasOne("Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("User2UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("WatchedMoviesMovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Api.Models.Movie", b =>
                {
                    b.Navigation("Cast");
                });
#pragma warning restore 612, 618
        }
    }
}

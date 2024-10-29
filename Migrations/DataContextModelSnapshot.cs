﻿using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("AgeRestriction")
                        .HasColumnType("int");

                    b.Property<string>("Cast")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Duration")
                        .HasColumnType("double");

                    b.Property<string>("Genre")
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Rating")
                        .HasColumnType("double");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TrailerUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserMovieID")
                        .HasColumnType("int");

                    b.Property<int?>("UserMovieID1")
                        .HasColumnType("int");

                    b.Property<int?>("UserMovieID2")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("MovieID");

                    b.HasIndex("UserMovieID");

                    b.HasIndex("UserMovieID1");

                    b.HasIndex("UserMovieID2");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Api.Models.UserMovie", b =>
                {
                    b.Property<int>("UserMovieID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserMovieID"));

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserMovieID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Api.Models.Movie", b =>
                {
                    b.HasOne("Api.Models.UserMovie", null)
                        .WithMany("RecommendedMovies")
                        .HasForeignKey("UserMovieID");

                    b.HasOne("Api.Models.UserMovie", null)
                        .WithMany("WatchedMovies")
                        .HasForeignKey("UserMovieID1");

                    b.HasOne("Api.Models.UserMovie", null)
                        .WithMany("Watchlist")
                        .HasForeignKey("UserMovieID2");
                });

            modelBuilder.Entity("Api.Models.UserMovie", b =>
                {
                    b.Navigation("RecommendedMovies");

                    b.Navigation("WatchedMovies");

                    b.Navigation("Watchlist");
                });
#pragma warning restore 612, 618
        }
    }
}

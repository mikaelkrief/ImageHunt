﻿// <auto-generated />
using System;
using ImageHunt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageHunt.Migrations
{
    [DbContext(typeof(HuntContext))]
    [Migration("20181016192808_Games_belong_to_many_admins")]
    partial class Games_belong_to_many_admins
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ImageHunt.Model.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<DateTime?>("ExpirationTokenDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int>("Role");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ImageHunt.Model.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<double?>("MapCenterLat");

                    b.Property<double?>("MapCenterLng");

                    b.Property<int?>("MapZoom");

                    b.Property<string>("Name");

                    b.Property<int>("NbPlayerPenaltyThreshold");

                    b.Property<double>("NbPlayerPenaltyValue");

                    b.Property<int?>("PictureId");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ImageHunt.Model.GameAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Action");

                    b.Property<int?>("CorrectAnswerId");

                    b.Property<DateTime>("DateOccured");

                    b.Property<DateTime>("DateReviewed");

                    b.Property<int?>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsReviewed");

                    b.Property<bool>("IsValidated");

                    b.Property<double?>("Latitude");

                    b.Property<double?>("Longitude");

                    b.Property<int?>("NodeId");

                    b.Property<int?>("PictureId");

                    b.Property<int>("PointsEarned");

                    b.Property<int?>("ReviewerId");

                    b.Property<int?>("SelectedAnswerId");

                    b.Property<int?>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("CorrectAnswerId");

                    b.HasIndex("GameId");

                    b.HasIndex("NodeId");

                    b.HasIndex("PictureId");

                    b.HasIndex("ReviewerId");

                    b.HasIndex("SelectedAnswerId");

                    b.HasIndex("TeamId");

                    b.ToTable("GameActions");
                });

            modelBuilder.Entity("ImageHunt.Model.GameAdmin", b =>
                {
                    b.Property<int>("AdminId");

                    b.Property<int>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("AdminId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("GameAdmin");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Correct");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("NodeId");

                    b.Property<int?>("QuestionNodeId");

                    b.Property<string>("Response");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.HasIndex("QuestionNodeId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("Points");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Nodes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Node");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.ParentChildren", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChildrenId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ChildrenId");

                    b.HasIndex("ParentId");

                    b.ToTable("ParentChildren");
                });

            modelBuilder.Entity("ImageHunt.Model.Passcode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("NbRedeem");

                    b.Property<string>("Pass");

                    b.Property<int>("Points");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Passcodes");
                });

            modelBuilder.Entity("ImageHunt.Model.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ImageHunt.Model.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChatLogin");

                    b.Property<int?>("CurrentGameId");

                    b.Property<int?>("CurrentNodeId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("CurrentGameId");

                    b.HasIndex("CurrentNodeId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ImageHunt.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChatId");

                    b.Property<string>("Color");

                    b.Property<string>("Comment");

                    b.Property<string>("CultureInfo");

                    b.Property<int?>("CurrentNodeId");

                    b.Property<int?>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<int?>("PictureId");

                    b.HasKey("Id");

                    b.HasIndex("CurrentNodeId");

                    b.HasIndex("GameId");

                    b.HasIndex("PictureId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ImageHunt.Model.TeamPasscode", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<int>("PasscodeId");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("TeamId", "PasscodeId");

                    b.HasIndex("PasscodeId");

                    b.ToTable("TeamPasscode");
                });

            modelBuilder.Entity("ImageHunt.Model.TeamPlayer", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<int>("PlayerId");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("TeamId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("TeamPlayer");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.FirstNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");

                    b.Property<string>("Password");

                    b.ToTable("FirstNode");

                    b.HasDiscriminator().HasValue("FirstNode");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.LastNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");


                    b.ToTable("LastNode");

                    b.HasDiscriminator().HasValue("LastNode");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.ObjectNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");

                    b.Property<string>("Action");

                    b.ToTable("ObjectNode");

                    b.HasDiscriminator().HasValue("ObjectNode");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.PictureNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");

                    b.Property<int?>("ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("PictureNode");

                    b.HasDiscriminator().HasValue("PictureNode");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.QuestionNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");

                    b.Property<string>("Question");

                    b.ToTable("QuestionNode");

                    b.HasDiscriminator().HasValue("QuestionNode");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.TimerNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");

                    b.Property<int>("Delay");

                    b.ToTable("TimerNode");

                    b.HasDiscriminator().HasValue("TimerNode");
                });

            modelBuilder.Entity("ImageHunt.Model.Game", b =>
                {
                    b.HasOne("ImageHunt.Model.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");
                });

            modelBuilder.Entity("ImageHunt.Model.GameAction", b =>
                {
                    b.HasOne("ImageHunt.Model.Node.Answer", "CorrectAnswer")
                        .WithMany()
                        .HasForeignKey("CorrectAnswerId");

                    b.HasOne("ImageHunt.Model.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("ImageHunt.Model.Node.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId");

                    b.HasOne("ImageHunt.Model.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.HasOne("ImageHunt.Model.Admin", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId");

                    b.HasOne("ImageHunt.Model.Node.Answer", "SelectedAnswer")
                        .WithMany()
                        .HasForeignKey("SelectedAnswerId");

                    b.HasOne("ImageHunt.Model.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("ImageHunt.Model.GameAdmin", b =>
                {
                    b.HasOne("ImageHunt.Model.Admin", "Admin")
                        .WithMany("GameAdmins")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHunt.Model.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHunt.Model.Node.Answer", b =>
                {
                    b.HasOne("ImageHunt.Model.Node.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId");

                    b.HasOne("ImageHunt.Model.Node.QuestionNode")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionNodeId");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.Node", b =>
                {
                    b.HasOne("ImageHunt.Model.Game")
                        .WithMany("Nodes")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.ParentChildren", b =>
                {
                    b.HasOne("ImageHunt.Model.Node.Node", "Children")
                        .WithMany()
                        .HasForeignKey("ChildrenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHunt.Model.Node.Node", "Parent")
                        .WithMany("ChildrenRelation")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHunt.Model.Passcode", b =>
                {
                    b.HasOne("ImageHunt.Model.Game")
                        .WithMany("Passcodes")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("ImageHunt.Model.Player", b =>
                {
                    b.HasOne("ImageHunt.Model.Game", "CurrentGame")
                        .WithMany()
                        .HasForeignKey("CurrentGameId");

                    b.HasOne("ImageHunt.Model.Node.Node", "CurrentNode")
                        .WithMany()
                        .HasForeignKey("CurrentNodeId");
                });

            modelBuilder.Entity("ImageHunt.Model.Team", b =>
                {
                    b.HasOne("ImageHunt.Model.Node.Node", "CurrentNode")
                        .WithMany()
                        .HasForeignKey("CurrentNodeId");

                    b.HasOne("ImageHunt.Model.Game")
                        .WithMany("Teams")
                        .HasForeignKey("GameId");

                    b.HasOne("ImageHunt.Model.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");
                });

            modelBuilder.Entity("ImageHunt.Model.TeamPasscode", b =>
                {
                    b.HasOne("ImageHunt.Model.Passcode", "Passcode")
                        .WithMany()
                        .HasForeignKey("PasscodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHunt.Model.Team", "Team")
                        .WithMany("TeamPasscodes")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHunt.Model.TeamPlayer", b =>
                {
                    b.HasOne("ImageHunt.Model.Player", "Player")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHunt.Model.Team", "Team")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHunt.Model.Node.PictureNode", b =>
                {
                    b.HasOne("ImageHunt.Model.Picture", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using ImageHunt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageHunt.Migrations
{
    [DbContext(typeof(HuntContext))]
    partial class HuntContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ImageHuntCore.Model.Admin", b =>
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

            modelBuilder.Entity("ImageHuntCore.Model.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPublic");

                    b.Property<double?>("MapCenterLat");

                    b.Property<double?>("MapCenterLng");

                    b.Property<int?>("MapZoom");

                    b.Property<string>("Name");

                    b.Property<int>("NbPlayerPenaltyThreshold");

                    b.Property<double>("NbPlayerPenaltyValue");

                    b.Property<int?>("PictureId");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("PictureId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ImageHuntCore.Model.GameAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Action");

                    b.Property<string>("Answer");

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

            modelBuilder.Entity("ImageHuntCore.Model.GameAdmin", b =>
                {
                    b.Property<int>("AdminId");

                    b.Property<int>("GameId");

                    b.HasKey("AdminId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("GameAdmin");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Identity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Role");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("TelegramUser");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChoiceNodeId");

                    b.Property<bool>("Correct");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("NodeId");

                    b.Property<string>("Response");

                    b.HasKey("Id");

                    b.HasIndex("ChoiceNodeId");

                    b.HasIndex("NodeId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("GameId");

                    b.Property<int?>("ImageId");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("Points");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("ImageId");

                    b.ToTable("Nodes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Node");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.ParentChildren", b =>
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

            modelBuilder.Entity("ImageHuntCore.Model.Passcode", b =>
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

            modelBuilder.Entity("ImageHuntCore.Model.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CloudUrl");

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Player", b =>
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

            modelBuilder.Entity("ImageHuntCore.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Bonus");

                    b.Property<string>("ChatId");

                    b.Property<string>("ChatInviteUrl");

                    b.Property<string>("Code");

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

            modelBuilder.Entity("ImageHuntCore.Model.TeamPasscode", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<int>("PasscodeId");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("TeamId", "PasscodeId");

                    b.HasIndex("PasscodeId");

                    b.ToTable("TeamPasscode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.TeamPlayer", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<int>("PlayerId");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("TeamId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("TeamPlayer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.BonusNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<int>("BonusType");

                    b.Property<string>("Location");

                    b.HasDiscriminator().HasValue("BonusNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.ChoiceNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<string>("Choice");

                    b.HasDiscriminator().HasValue("ChoiceNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.FirstNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<string>("Password");

                    b.HasDiscriminator().HasValue("FirstNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.HiddenNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<string>("LocationHint");

                    b.HasDiscriminator().HasValue("HiddenNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.LastNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.HasDiscriminator().HasValue("LastNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.ObjectNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<string>("Action");

                    b.HasDiscriminator().HasValue("ObjectNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.PictureNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.HasDiscriminator().HasValue("PictureNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.QuestionNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<string>("Answer");

                    b.Property<bool>("CanOverride");

                    b.Property<string>("Question");

                    b.HasDiscriminator().HasValue("QuestionNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.TimerNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.Property<int>("Delay");

                    b.HasDiscriminator().HasValue("TimerNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.WaypointNode", b =>
                {
                    b.HasBaseType("ImageHuntCore.Model.Node.Node");

                    b.HasDiscriminator().HasValue("WaypointNode");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Game", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.GameAction", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Node.Answer", "CorrectAnswer")
                        .WithMany()
                        .HasForeignKey("CorrectAnswerId");

                    b.HasOne("ImageHuntCore.Model.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("ImageHuntCore.Model.Node.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId");

                    b.HasOne("ImageHuntCore.Model.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.HasOne("ImageHuntCore.Model.Admin", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId");

                    b.HasOne("ImageHuntCore.Model.Node.Answer", "SelectedAnswer")
                        .WithMany()
                        .HasForeignKey("SelectedAnswerId");

                    b.HasOne("ImageHuntCore.Model.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.GameAdmin", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Admin", "Admin")
                        .WithMany("GameAdmins")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHuntCore.Model.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.Answer", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Node.ChoiceNode")
                        .WithMany("Answers")
                        .HasForeignKey("ChoiceNodeId");

                    b.HasOne("ImageHuntCore.Model.Node.Node", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.Node", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Game")
                        .WithMany("Nodes")
                        .HasForeignKey("GameId");

                    b.HasOne("ImageHuntCore.Model.Picture", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Node.ParentChildren", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Node.Node", "Children")
                        .WithMany()
                        .HasForeignKey("ChildrenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHuntCore.Model.Node.Node", "Parent")
                        .WithMany("ChildrenRelation")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHuntCore.Model.Passcode", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Game")
                        .WithMany("Passcodes")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Player", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Game", "CurrentGame")
                        .WithMany()
                        .HasForeignKey("CurrentGameId");

                    b.HasOne("ImageHuntCore.Model.Node.Node", "CurrentNode")
                        .WithMany()
                        .HasForeignKey("CurrentNodeId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.Team", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Node.Node", "CurrentNode")
                        .WithMany()
                        .HasForeignKey("CurrentNodeId");

                    b.HasOne("ImageHuntCore.Model.Game")
                        .WithMany("Teams")
                        .HasForeignKey("GameId");

                    b.HasOne("ImageHuntCore.Model.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");
                });

            modelBuilder.Entity("ImageHuntCore.Model.TeamPasscode", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Passcode", "Passcode")
                        .WithMany()
                        .HasForeignKey("PasscodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHuntCore.Model.Team", "Team")
                        .WithMany("TeamPasscodes")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ImageHuntCore.Model.TeamPlayer", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Player", "Player")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHuntCore.Model.Team", "Team")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Identity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Identity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ImageHuntCore.Model.Identity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ImageHuntCore.Model.Identity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

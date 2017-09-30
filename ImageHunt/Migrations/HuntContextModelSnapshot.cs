﻿// <auto-generated />
using ImageHunt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace ImageHunt.Migrations
{
    [DbContext(typeof(HuntContext))]
    partial class HuntContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("ImageHunt.Model.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<DateTime?>("ExpirationTokenDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ImageHunt.Model.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdminId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<double?>("MapCenterLat");

                    b.Property<double?>("MapCenterLng");

                    b.Property<int?>("MapZoom");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int?>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Nodes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Node");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.ParentChildren", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChildrenId");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ChildrenId");

                    b.HasIndex("ParentId");

                    b.ToTable("ParentChildren");
                });

            modelBuilder.Entity("ImageHunt.Model.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsDeleted");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ImageHunt.Model.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChatLogin");

                    b.Property<int?>("CurrentNodeId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("StartTime");

                    b.Property<int?>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("CurrentNodeId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ImageHunt.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GameId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ImageHunt.Model.Node.FirstNode", b =>
                {
                    b.HasBaseType("ImageHunt.Model.Node.Node");


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
                    b.HasOne("ImageHunt.Model.Admin")
                        .WithMany("Games")
                        .HasForeignKey("AdminId");
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

            modelBuilder.Entity("ImageHunt.Model.Player", b =>
                {
                    b.HasOne("ImageHunt.Model.Node.Node", "CurrentNode")
                        .WithMany()
                        .HasForeignKey("CurrentNodeId");

                    b.HasOne("ImageHunt.Model.Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("ImageHunt.Model.Team", b =>
                {
                    b.HasOne("ImageHunt.Model.Game")
                        .WithMany("Teams")
                        .HasForeignKey("GameId");
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

﻿// <auto-generated />
using System;
using Gruggbot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gruggbot.Data.Migrations
{
    [DbContext(typeof(GruggbotContext))]
    partial class GruggbotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0-rc.2.20475.6");

            modelBuilder.Entity("Gruggbot.DomainModel.Command", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CommandAlias", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Alias")
                        .HasColumnType("TEXT");

                    b.Property<int>("CommandId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CommandId");

                    b.ToTable("Aliases");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CommandMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommandId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Sequence")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CommandId");

                    b.ToTable("CommandMessages");

                    b.HasDiscriminator<string>("MessageType").HasValue("CommandMessage");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.ImageDetails", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ImageDetails");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CountdownCommand", b =>
                {
                    b.HasBaseType("Gruggbot.DomainModel.Command");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Event")
                        .HasColumnType("TEXT");

                    b.ToTable("CountdownCommands");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CommandMessageImage", b =>
                {
                    b.HasBaseType("Gruggbot.DomainModel.CommandMessage");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("ImageId");

                    b.HasDiscriminator().HasValue("ImageMessage");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CommandAlias", b =>
                {
                    b.HasOne("Gruggbot.DomainModel.Command", null)
                        .WithMany("Aliases")
                        .HasForeignKey("CommandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CommandMessage", b =>
                {
                    b.HasOne("Gruggbot.DomainModel.Command", null)
                        .WithMany("Messages")
                        .HasForeignKey("CommandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CountdownCommand", b =>
                {
                    b.HasOne("Gruggbot.DomainModel.Command", null)
                        .WithOne()
                        .HasForeignKey("Gruggbot.DomainModel.CountdownCommand", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Gruggbot.DomainModel.CommandMessageImage", b =>
                {
                    b.HasOne("Gruggbot.DomainModel.ImageDetails", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Gruggbot.DomainModel.Command", b =>
                {
                    b.Navigation("Aliases");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}

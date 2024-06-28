﻿// <auto-generated />
using System;
using EmailSender.Data.EmailContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmailSender.Migrations
{
    [DbContext(typeof(EmailDbContext))]
    partial class EmailDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EmailSender.Data.Entities.CampaignCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CampaignCount")
                        .HasColumnType("int");

                    b.Property<string>("CampaignName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmailLogId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("CampaignCommands");
                });

            modelBuilder.Entity("EmailSender.Data.Entities.EmailCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CampaignCommandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CommandNumberOfEmails")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EmailCountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EmailLogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CampaignCommandId");

                    b.HasIndex("EmailCountId")
                        .IsUnique();

                    b.ToTable("EmailCommands");
                });

            modelBuilder.Entity("EmailSender.Data.Entities.EmailCount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailCounts");
                });

            modelBuilder.Entity("EmailSender.Data.Entities.EmailLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CampaignCommandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EmailCommandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailCommandLogMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CampaignCommandId")
                        .IsUnique()
                        .HasFilter("[CampaignCommandId] IS NOT NULL");

                    b.HasIndex("EmailCommandId")
                        .IsUnique()
                        .HasFilter("[EmailCommandId] IS NOT NULL");

                    b.ToTable("EmailLogs");
                });

            modelBuilder.Entity("EmailSender.Data.Entities.EmailCommand", b =>
                {
                    b.HasOne("EmailSender.Data.Entities.CampaignCommand", "CampaignCommand")
                        .WithMany("EmailCommands")
                        .HasForeignKey("CampaignCommandId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmailSender.Data.Entities.EmailCount", "EmailCount")
                        .WithOne()
                        .HasForeignKey("EmailSender.Data.Entities.EmailCommand", "EmailCountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CampaignCommand");

                    b.Navigation("EmailCount");
                });

            modelBuilder.Entity("EmailSender.Data.Entities.EmailLog", b =>
                {
                    b.HasOne("EmailSender.Data.Entities.CampaignCommand", "CampaignCommand")
                        .WithOne("EmailLog")
                        .HasForeignKey("EmailSender.Data.Entities.EmailLog", "CampaignCommandId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EmailSender.Data.Entities.EmailCommand", "EmailCommand")
                        .WithOne("EmailLog")
                        .HasForeignKey("EmailSender.Data.Entities.EmailLog", "EmailCommandId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CampaignCommand");

                    b.Navigation("EmailCommand");
                });

            modelBuilder.Entity("EmailSender.Data.Entities.CampaignCommand", b =>
                {
                    b.Navigation("EmailCommands");

                    b.Navigation("EmailLog")
                        .IsRequired();
                });

            modelBuilder.Entity("EmailSender.Data.Entities.EmailCommand", b =>
                {
                    b.Navigation("EmailLog")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

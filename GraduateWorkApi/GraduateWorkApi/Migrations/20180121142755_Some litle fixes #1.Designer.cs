﻿// <auto-generated />
using GraduateWorkApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace GraduateWorkApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180121142755_Some litle fixes #1")]
    partial class Somelitlefixes1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EntityModels.Entitys.CertificateOfSecondaryEducationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("AverageMark");

                    b.Property<int?>("EntrantId");

                    b.Property<string>("FullNameOfTheEducationalInstitution");

                    b.Property<string>("SeriaNumber");

                    b.Property<DateTime>("YearOfIssue");

                    b.HasKey("Id");

                    b.HasIndex("EntrantId")
                        .IsUnique()
                        .HasFilter("[EntrantId] IS NOT NULL");

                    b.ToTable("CertificateOfSecondaryEducations");
                });

            modelBuilder.Entity("EntityModels.Entitys.CertificateOfTestingEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EntrantId");

                    b.Property<float>("FirstMark");

                    b.Property<string>("FirstSubject");

                    b.Property<float>("FourthMark");

                    b.Property<string>("FourthSubject");

                    b.Property<float>("SecondMark");

                    b.Property<string>("SecondSubject");

                    b.Property<string>("SerialNumber");

                    b.Property<float>("ThirdMark");

                    b.Property<string>("ThirdSubject");

                    b.Property<DateTime>("YearOfIssue");

                    b.HasKey("Id");

                    b.HasIndex("EntrantId")
                        .IsUnique()
                        .HasFilter("[EntrantId] IS NOT NULL");

                    b.ToTable("CertificateOfTestings");
                });

            modelBuilder.Entity("EntityModels.Entitys.EntrantEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BDay");

                    b.Property<string>("Name");

                    b.Property<string>("Surname");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Entrants");
                });

            modelBuilder.Entity("EntityModels.Entitys.SpecialityEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("AdditionalFactor");

                    b.Property<string>("Code");

                    b.Property<int>("CountOfStatePlaces");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Specialtys");
                });

            modelBuilder.Entity("EntityModels.Entitys.StatementEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EntrantId");

                    b.Property<float>("ExtraScore");

                    b.Property<float>("TotalScore");

                    b.Property<int>("UniversityId");

                    b.HasKey("Id");

                    b.HasIndex("EntrantId");

                    b.HasIndex("UniversityId");

                    b.ToTable("Statements");
                });

            modelBuilder.Entity("EntityModels.Entitys.UniversityEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullName");

                    b.Property<string>("LevelOfAccreditation");

                    b.HasKey("Id");

                    b.ToTable("Universitys");
                });

            modelBuilder.Entity("EntityModels.Entitys.UniversitySpeciality", b =>
                {
                    b.Property<int>("SpecialtyId");

                    b.Property<int>("UniversityId");

                    b.HasKey("SpecialtyId", "UniversityId");

                    b.HasIndex("UniversityId");

                    b.ToTable("UniversitySpeciality");
                });

            modelBuilder.Entity("EntityModels.Entitys.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EntityModels.Entitys.CertificateOfSecondaryEducationEntity", b =>
                {
                    b.HasOne("EntityModels.Entitys.EntrantEntity", "Entrant")
                        .WithOne("CertificateOfSecondaryEducation")
                        .HasForeignKey("EntityModels.Entitys.CertificateOfSecondaryEducationEntity", "EntrantId");
                });

            modelBuilder.Entity("EntityModels.Entitys.CertificateOfTestingEntity", b =>
                {
                    b.HasOne("EntityModels.Entitys.EntrantEntity", "Entrant")
                        .WithOne("CertificateOfTesting")
                        .HasForeignKey("EntityModels.Entitys.CertificateOfTestingEntity", "EntrantId");
                });

            modelBuilder.Entity("EntityModels.Entitys.EntrantEntity", b =>
                {
                    b.HasOne("EntityModels.Entitys.UserEntity", "User")
                        .WithOne("Entrant")
                        .HasForeignKey("EntityModels.Entitys.EntrantEntity", "UserId");
                });

            modelBuilder.Entity("EntityModels.Entitys.StatementEntity", b =>
                {
                    b.HasOne("EntityModels.Entitys.EntrantEntity", "Entrant")
                        .WithMany("Statements")
                        .HasForeignKey("EntrantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EntityModels.Entitys.UniversityEntity", "University")
                        .WithMany("Statements")
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EntityModels.Entitys.UniversitySpeciality", b =>
                {
                    b.HasOne("EntityModels.Entitys.SpecialityEntity", "Specialty")
                        .WithMany("UniversitySpecialities")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EntityModels.Entitys.UniversityEntity", "University")
                        .WithMany("UniversitySpecialities")
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
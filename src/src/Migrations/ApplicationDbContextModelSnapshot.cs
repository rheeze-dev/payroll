﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using src.Data;
using System;

namespace src.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

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

            modelBuilder.Entity("src.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<int>("BasicPay");

                    b.Property<string>("BirthDate");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<string>("IdNumber");

                    b.Property<bool>("IsSuperAdmin");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Position");

                    b.Property<string>("ProfilePictureUrl")
                        .HasMaxLength(250);

                    b.Property<string>("Role");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("WallpaperPictureUrl")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("src.Models.Attendance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("EditorTimeIn");

                    b.Property<string>("EditorTimeOut");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<string>("IdNumber");

                    b.Property<int>("NumberOfMinOT");

                    b.Property<int>("NumberOfMinSunday");

                    b.Property<int>("NumberOfMinTardiness");

                    b.Property<int>("NumberOfMinWorked");

                    b.Property<string>("Remarks");

                    b.Property<DateTime?>("TimeIn");

                    b.Property<DateTime?>("TimeOut");

                    b.HasKey("Id");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("src.Models.Contact", b =>
                {
                    b.Property<Guid>("contactId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("applicationUserId");

                    b.Property<string>("contactName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<Guid>("customerId");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("linkedin")
                        .HasMaxLength(100);

                    b.Property<string>("phone")
                        .HasMaxLength(20);

                    b.Property<string>("secondaryEmail")
                        .HasMaxLength(100);

                    b.Property<string>("thumbUrl")
                        .HasMaxLength(255);

                    b.Property<string>("website")
                        .HasMaxLength(100);

                    b.HasKey("contactId");

                    b.HasIndex("applicationUserId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("src.Models.CurrentLedger", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddAdjustment");

                    b.Property<double>("AmountOT");

                    b.Property<double>("AmountRH");

                    b.Property<double>("AmountSH");

                    b.Property<double>("AmountSundays");

                    b.Property<double>("AmountTardiness");

                    b.Property<int>("BasicPay");

                    b.Property<int>("CashOut");

                    b.Property<int>("Charges1");

                    b.Property<int>("Charges2");

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime?>("DateAndTime");

                    b.Property<int>("DaysOfWorkBP");

                    b.Property<string>("Editor");

                    b.Property<string>("Email");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<double>("GrossPay");

                    b.Property<double>("GrossPayPayslip");

                    b.Property<string>("IdNumber");

                    b.Property<int>("LessAdjustment");

                    b.Property<double>("LoanAmount");

                    b.Property<double>("LoanBalance");

                    b.Property<bool>("MidMonth");

                    b.Property<double>("NetAmountPaid");

                    b.Property<int>("NumberOfDaysRH");

                    b.Property<int>("NumberOfHrsSH");

                    b.Property<int>("NumberOfMinOT");

                    b.Property<int>("NumberOfMinSundays");

                    b.Property<int>("NumberOfMinTardiness");

                    b.Property<double>("PagibigEmployee");

                    b.Property<double>("PagibigEmployer");

                    b.Property<int?>("PaymentPlan");

                    b.Property<double>("PhilHealthEmployee");

                    b.Property<double>("PhilHealthEmployer");

                    b.Property<double>("SSSEmployee");

                    b.Property<double>("SSSEmployer");

                    b.Property<int>("SalaryLoan");

                    b.Property<bool>("SalaryLoanChecker");

                    b.Property<double>("TotalAmountBP");

                    b.Property<double>("TotalDeductions");

                    b.HasKey("Id");

                    b.ToTable("CurrentLedger");
                });

            modelBuilder.Entity("src.Models.Employees", b =>
                {
                    b.Property<string>("IdNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<int?>("BasicPay");

                    b.Property<string>("BirthDate");

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime?>("DateTimeChecker");

                    b.Property<DateTime?>("DateTimeEdited");

                    b.Property<string>("Editor");

                    b.Property<string>("Email");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<string>("Id");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Role");

                    b.Property<DateTime?>("TimeInChecker");

                    b.Property<int>("TotalTimeIn");

                    b.Property<int>("TotalTimeOut");

                    b.HasKey("IdNumber");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("src.Models.EmployersDeduction", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime>("Date");

                    b.Property<double>("PagibigTotal");

                    b.Property<double>("PhilhealthTotal");

                    b.Property<double>("SssTotal");

                    b.HasKey("Id");

                    b.ToTable("EmployersDeduction");
                });

            modelBuilder.Entity("src.Models.Organization", b =>
                {
                    b.Property<Guid>("organizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<string>("description")
                        .HasMaxLength(200);

                    b.Property<string>("organizationName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("organizationOwnerId");

                    b.Property<string>("thumbUrl")
                        .HasMaxLength(255);

                    b.HasKey("organizationId");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("src.Models.SalaryLedger", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddAdjustment");

                    b.Property<double>("AmountOT");

                    b.Property<double>("AmountRH");

                    b.Property<double>("AmountSH");

                    b.Property<double>("AmountSundays");

                    b.Property<double>("AmountTardiness");

                    b.Property<int>("BasicPay");

                    b.Property<int>("CashOut");

                    b.Property<int>("Charges1");

                    b.Property<int>("Charges2");

                    b.Property<DateTime>("CreateAt");

                    b.Property<string>("CreateBy");

                    b.Property<DateTime?>("DateAndTime");

                    b.Property<int>("DaysOfWorkBP");

                    b.Property<string>("Editor");

                    b.Property<string>("Email");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<double>("GrossPay");

                    b.Property<double>("GrossPayPayslip");

                    b.Property<string>("IdNumber");

                    b.Property<int>("LessAdjustment");

                    b.Property<double>("LoanAmount");

                    b.Property<double>("LoanBalance");

                    b.Property<bool>("MidMonth");

                    b.Property<double>("NetAmountPaid");

                    b.Property<int>("NumberOfDaysRH");

                    b.Property<int>("NumberOfHrsSH");

                    b.Property<int>("NumberOfMinOT");

                    b.Property<int>("NumberOfMinSundays");

                    b.Property<int>("NumberOfMinTardiness");

                    b.Property<double>("PagibigEmployee");

                    b.Property<double>("PagibigEmployer");

                    b.Property<int?>("PaymentPlan");

                    b.Property<double>("PhilHealthEmployee");

                    b.Property<double>("PhilHealthEmployer");

                    b.Property<double>("SSSEmployee");

                    b.Property<double>("SSSEmployer");

                    b.Property<int>("SalaryLoan");

                    b.Property<bool>("SalaryLoanChecker");

                    b.Property<double>("TotalAmountBP");

                    b.Property<double>("TotalDeductions");

                    b.HasKey("Id");

                    b.ToTable("SalaryLedger");
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
                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("src.Models.ApplicationUser")
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

                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("src.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("src.Models.Contact", b =>
                {
                    b.HasOne("src.Models.ApplicationUser", "applicationUser")
                        .WithMany()
                        .HasForeignKey("applicationUserId");
                });
#pragma warning restore 612, 618
        }
    }
}

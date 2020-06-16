﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NWBA_API.Data;

namespace NWBA_API.Migrations
{
    [DbContext(typeof(NwbaContext))]
    partial class NwbaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NWBA_API.Models.Account", b =>
                {
                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("money");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("FreeTransactions")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("AccountNumber");

                    b.HasIndex("CustomerID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("NWBA_API.Models.BillPay", b =>
                {
                    b.Property<int>("BillPayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PayeeID")
                        .HasColumnType("int");

                    b.Property<int>("Period")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScheduleDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BillPayID");

                    b.ToTable("BillPay");
                });

            modelBuilder.Entity("NWBA_API.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.Property<int>("TaxFileNumber")
                        .HasColumnType("int");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("NWBA_API.Models.Login", b =>
                {
                    b.Property<string>("LoginID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("LoginID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Logins");

                    b.HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8");

                    b.HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
                });

            modelBuilder.Entity("NWBA_API.Models.LoginAttempt", b =>
                {
                    b.Property<int>("LoginAttemptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("datetime2");

                    b.HasKey("LoginAttemptID");

                    b.ToTable("LoginAttempts");
                });

            modelBuilder.Entity("NWBA_API.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int?>("DestinationAccountNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifyTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TransactionTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("TransactionID");

                    b.HasIndex("AccountNumber");

                    b.ToTable("Transactions");

                    b.HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");
                });

            modelBuilder.Entity("NWBA_API.Models.Account", b =>
                {
                    b.HasOne("NWBA_API.Models.Customer", null)
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NWBA_API.Models.Login", b =>
                {
                    b.HasOne("NWBA_API.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NWBA_API.Models.Transaction", b =>
                {
                    b.HasOne("NWBA_API.Models.Account", "DestinationAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

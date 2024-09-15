﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace tgropa.Core.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20240915200332_M7")]
    partial class M7
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("tgropa.Data.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("tgropa.Data.InvoiceItem", b =>
                {
                    b.Property<int>("InvoiceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("InvoiceId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("InvoiceItems");
                });

            modelBuilder.Entity("tgropa.Data.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("InitialPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("RetailPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("WholesalePrice")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("tgropa.Data.InvoiceItem", b =>
                {
                    b.HasOne("tgropa.Data.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tgropa.Data.Item", "Item")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("tgropa.Data.Invoice", b =>
                {
                    b.Navigation("InvoiceItems");
                });

            modelBuilder.Entity("tgropa.Data.Item", b =>
                {
                    b.Navigation("InvoiceItems");
                });
#pragma warning restore 612, 618
        }
    }
}

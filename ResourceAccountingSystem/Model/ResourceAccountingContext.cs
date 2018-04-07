using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ResourceAccountingSystem.Model
{
    public partial class ResourceAccountingContext : DbContext
    {
        public virtual DbSet<House> House { get; set; }
        public virtual DbSet<Meter> Meter { get; set; }
        public virtual DbSet<MeterReading> MeterReading { get; set; }
        public virtual DbSet<Street> Street { get; set; }


        public ResourceAccountingContext(DbContextOptions<ResourceAccountingContext> options)
            : base(options) { }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ResourceAccounting;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>(entity =>
            {
                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.Street)
                    .WithMany(p => p.House)
                    .HasForeignKey(d => d.StreetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_House_Street_Id");
            });

            modelBuilder.Entity<Meter>(entity =>
            {
                entity.HasKey(e => e.SerialNumber);

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Meter)
                    .HasForeignKey(d => d.HouseId)
                    .HasConstraintName("FK_Meter_House_Id");
            });

            modelBuilder.Entity<MeterReading>(entity =>
            {
                entity.Property(e => e.MeterSerialNumber)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ReadingDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.MeterSerialNumberNavigation)
                    .WithMany(p => p.MeterReading)
                    .HasForeignKey(d => d.MeterSerialNumber)
                    .HasConstraintName("FK_MeterReading_Meter_SerialNumber");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}

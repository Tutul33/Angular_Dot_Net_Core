using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccessLayer.DBContext
{
    public partial class InfonetDBContext : DbContext
    {
        public InfonetDBContext()
        {
        }

        public InfonetDBContext(DbContextOptions<InfonetDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCity> TblCities { get; set; }
        public virtual DbSet<TblCountry> TblCountries { get; set; }
        public virtual DbSet<TblPersonalAttachment> TblPersonalAttachments { get; set; }
        public virtual DbSet<TblPersonalInfo> TblPersonalInfos { get; set; }
        public virtual DbSet<TblPersonalSkill> TblPersonalSkills { get; set; }
        public virtual DbSet<TblSkill> TblSkills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=TUTUL_RGL_RDP\\DEV_DB;Database=InfonetDB;User Id=sa; Password=@sa12345#;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblCity>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.ToTable("tblCity");

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCities)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tblCity_tblCountry");
            });

            modelBuilder.Entity<TblCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("tblCountry");

                entity.Property(e => e.CountryName).HasMaxLength(100);
            });

            modelBuilder.Entity<TblPersonalAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity.ToTable("tblPersonalAttachment");

                entity.Property(e => e.FileName).HasMaxLength(250);

                entity.Property(e => e.FilePath).HasMaxLength(250);
            });

            modelBuilder.Entity<TblPersonalInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblPersonalInfo");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserCode)
                    .HasMaxLength(33)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('PIF'+CONVERT([varchar],floor(rand([UserID])*(1000000)-(1))))", false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblPersonalInfos)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_tblPersonalInfo_tblCity");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblPersonalInfos)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tblPersonalInfo_tblCountry");
            });

            modelBuilder.Entity<TblPersonalSkill>(entity =>
            {
                entity.HasKey(e => e.UserSkillId);

                entity.ToTable("tblPersonalSkill");
            });

            modelBuilder.Entity<TblSkill>(entity =>
            {
                entity.HasKey(e => e.SkillId);

                entity.ToTable("tblSkill");

                entity.Property(e => e.SkillName).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

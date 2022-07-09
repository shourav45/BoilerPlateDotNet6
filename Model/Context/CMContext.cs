using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Model.Models;

namespace Model.Context
{
    public partial class CMContext : DbContext
    {
        public CMContext()
        {
        }

        public CMContext(DbContextOptions<CMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TodoAttachment> TodoAttachments { get; set; } = null!;
        public virtual DbSet<TodoMaster> TodoMasters { get; set; } = null!;
        public virtual DbSet<UserAccount> UserAccounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=IT-PC3\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoAttachment>(entity =>
            {
                entity.ToTable("TodoAttachment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FileExtension)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileTitle)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TodoId).HasColumnName("TodoID");
            });

            modelBuilder.Entity<TodoMaster>(entity =>
            {
                entity.ToTable("TodoMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AssignedOn).HasColumnType("datetime");

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FeedBack)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccount");

                entity.HasIndex(e => e.UserName, "Unique_UserName_UserAccount")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cif)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

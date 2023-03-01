using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace TatBlog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(500);
            builder.Property(a => a.ShortDescription).IsRequired().HasMaxLength(5000);
            builder.Property(a => a.Description).IsRequired().HasMaxLength(5000);
            builder.Property(a => a.UrlSlug).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Meta).IsRequired().HasMaxLength(1000);
            builder.Property(a => a.ImageUrl).HasMaxLength(1000);
            builder.Property(a => a.ViewCount).IsRequired().HasMaxLength(0);
            builder.Property(a => a.Published).IsRequired().HasDefaultValue(false);
            builder.Property(a => a.PostedDate).HasColumnType("datetime");
            builder.Property(a => a.ModifiedDate).HasColumnType("datetime");

            builder.HasOne(a => a.Category).
                WithMany(b => b.Posts).
                HasForeignKey(a => a.CategoryId).
                HasConstraintName("FK_Posts_Categories").
                OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Author).
                WithMany(b => b.Posts).
                HasForeignKey(a => a.AuthorId).
                HasConstraintName("FK_Posts_Authors").
                OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Tags).
                WithMany(t => t.Posts).
                UsingEntity(pt => pt.ToTable("PostTags"));











        }
    }
}

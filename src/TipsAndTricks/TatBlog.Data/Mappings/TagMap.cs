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

    public class TagMap : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(a=>a.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Description).HasMaxLength(500);//tại sao tháng này lại 28 
            builder.Property(a => a.UrlSlug).IsRequired().HasMaxLength(50);
            


        }
    }
}

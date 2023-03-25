using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToTable("Subscribers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(500); 
            builder.Property(x=>x.SignUpDate).HasColumnType("datetime");
            builder.Property(x => x.UnSignUpDate).HasColumnType("datetime");
            builder.Property(x => x.Reason).HasMaxLength(1000);
            builder.Property(x => x.Flag)
                .HasDefaultValue(false)
                .HasColumnType("bit");
            builder.Property(x => x.Status).HasColumnType("bit");
            builder.Property(x => x.Notes).HasMaxLength(1000);





        
        }
    }
}

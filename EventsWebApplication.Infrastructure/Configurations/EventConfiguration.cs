using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                    .HasMaxLength(500);

            builder.Property(e => e.Place)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.EventDateTime)
                    .IsRequired();

            builder.Property(e => e.EventDateTime)
                    .IsRequired();

            builder.Property(e => e.ParticipantsMaxCount)
                    .IsRequired();
        }
    }
}

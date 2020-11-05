using Gruggbot.DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Data.EntityConfiguration
{
    public class CommandMessageConfiguration : IEntityTypeConfiguration<CommandMessage>
    {
        public void Configure(EntityTypeBuilder<CommandMessage> builder)
        {
            builder.ToTable("CommandMessages")
                .HasKey(m => m.ID);

            builder.HasDiscriminator<string>("MessageType")
                .HasValue<CommandMessage>("CommandMessage")
                .HasValue<CommandMessageImage>("ImageMessage");
        }
    }
}

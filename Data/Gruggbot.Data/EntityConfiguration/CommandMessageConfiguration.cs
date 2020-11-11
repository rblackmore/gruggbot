namespace Gruggbot.Data.EntityConfiguration
{
    using Gruggbot.DomainModel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

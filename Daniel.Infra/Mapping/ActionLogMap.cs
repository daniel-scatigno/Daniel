using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Daniel.Domain.Models;
namespace Daniel.Infra.Mapping
{
    public class ActionLogMap : IEntityTypeConfiguration<ActionLog>
    {
        public void Configure(EntityTypeBuilder<ActionLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Action).HasConversion<string>().HasMaxLength(100);
            builder.Property(x => x.Change).HasMaxLength(4000);
            builder.Property(x => x.ObjectType).HasMaxLength(300);
            builder.Property(x => x.Login).HasMaxLength(300);
            builder.Property(x=>x.IdUser);
            builder.Property(x=>x.IdRecord);
            
            
        }
    }
}
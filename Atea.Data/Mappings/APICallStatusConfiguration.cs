using Atea.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atea.Data.Mappings
{
    public class APICallStatusConfiguration : IEntityTypeConfiguration<APICallStatus>
    {
        public void Configure(EntityTypeBuilder<APICallStatus> builder)
        {
            builder.ToTable("tabAPICallStatus");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(x => x.Status).HasColumnName("status");
            builder.Property(x => x.BlobGuid).HasColumnName("blobguid");
            builder.Property(x => x.UtcTimestamp).HasColumnName("utctimestamp");
        }
    }
}
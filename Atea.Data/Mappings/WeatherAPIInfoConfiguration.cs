using Atea.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atea.Data.Mappings
{
    public class WeatherAPIInfoConfiguration : IEntityTypeConfiguration<WeatherAPIInfo>
    {
        public void Configure(EntityTypeBuilder<WeatherAPIInfo> builder)
        {
            builder.ToTable("tabWeatherAPIInfo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(x => x.CityId).HasColumnName("cityid");
            builder.Property(x => x.State).HasColumnName("state");
            builder.Property(x => x.Temperature).HasColumnName("temperature");
            builder.Property(x => x.WindSpeed).HasColumnName("windspeed");
            builder.Property(x => x.UtcTimestamp).HasColumnName("utctimestamp");
        }
    }
}
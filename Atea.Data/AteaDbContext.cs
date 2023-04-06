using Atea.Data.Entities;
using Atea.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Atea.Data
{
    public partial class AteaDbContext : DbContext
    {
        public AteaDbContext()
        {

        }

        public AteaDbContext(DbContextOptions<AteaDbContext> options) : base(options)
        {

        }

        public virtual DbSet<APICallStatus> APICallStatuses { get; set; }
        public virtual DbSet<WeatherAPIInfo> WeatherAPIInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new APICallStatusConfiguration());
            modelBuilder.ApplyConfiguration(new WeatherAPIInfoConfiguration());
        }
    }
}
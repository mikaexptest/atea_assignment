using Atea.Data;
using Atea.Models.Models;
using Atea.Services.Interfaces;
using Atea.Services.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Task1.Startup))]
namespace Task1
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string dbConnectionString = Environment.GetEnvironmentVariable("ATEA_DB_CONNECTION_STRING");
            builder.Services.AddDbContext<AteaDbContext>(options => options.UseSqlServer(dbConnectionString));

            //App Settings.
            string storageAccountName = Environment.GetEnvironmentVariable("StorageAccountName") ?? throw new InvalidDataException("Missing Environment Variable StorageAccountName!");
            string storageContainerName = Environment.GetEnvironmentVariable("StorageContainerName") ?? throw new InvalidDataException("Missing Environment Variable StorageContainerName!");
            string storageContainerSASToken = Environment.GetEnvironmentVariable("StorageContainerSASToken") ?? throw new InvalidDataException("Missing Environment Variable StorageContainerSASToken!");
            builder.Services.AddSingleton(new AppSettingsModel
            {
                StorageAccountName = storageAccountName,
                StorageContainerName = storageContainerName,
                StorageContainerSASToken = storageContainerSASToken
            });

            //Data Services.
            builder.Services.AddScoped<IAPICallStatusService, APICallStatusService>();
        }
    }
}
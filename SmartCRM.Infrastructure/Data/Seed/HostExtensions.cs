using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace SmartCRM.Infrastructure.Data.Seed
{
    public static class HostExtensions
    {
        public static async Task SeedDatabaseAsync(this IHost host)
        {
            await DataSeeder.SeedAsync(host.Services);
        }
    }
}
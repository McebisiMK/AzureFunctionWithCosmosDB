using AzureFunctionCosmoDB;
using AzureFunctionCosmosDB.Data.Repositories.Students;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctionCosmoDB
{
    class Startup : FunctionsStartup
    {
        private readonly IConfiguration _configuration;
        public Startup()
        {
            var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..");
            _configuration = new ConfigurationBuilder()
                            .SetBasePath(basePath)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IStudentRepository>(InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult());
        }

        private async Task<StudentRepository> InitializeCosmosClientInstanceAsync()
        {
            string databaseName = _configuration["DatabaseName"];
            string containerName = _configuration["ContainerName"];
            string account = _configuration["URI"];
            string key = _configuration["Key"];
            var clientBuilder = new CosmosClientBuilder(account, key);
            var client = clientBuilder
                          .WithConnectionModeDirect()
                          .Build();

            var studentRepository = new StudentRepository(client, databaseName, containerName);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return studentRepository;
        }
    }
}

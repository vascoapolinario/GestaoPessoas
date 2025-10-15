using GestaoPessoas.DataSources;
using GestaoPessoas.Dtos;
using GestaoPessoas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestaoPessoasTests.Tests
{
    [TestClass]
    [DoNotParallelize]
    public sealed class WorkerServicePostGresTests : WorkerServiceTestsBase 
    {

        [TestInitialize]
        public void Initialize()
        {
            applicationDomain = new TestApplicationDomain();

            string? _connectionString = applicationDomain.configuration.GetConnectionString("DefaultConnection");
            if (_connectionString == null) Assert.Inconclusive("Não foi possível obter a connection string de configuração.");

            applicationDomain.Services.AddDbContext<GestaoPessoasDbContext>(o => o.UseNpgsql(_connectionString));
            applicationDomain.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
            service = applicationDomain.ServiceProvider.GetRequiredService<IWorkerService>();

            string? BackupPath = applicationDomain.configuration.GetValue<string>("PostGresWorkerService:FilePath");
            if (BackupPath == null) throw new Exception("Não foi possível obter o caminho do ficheiro de configuração.");

            string sql = File.ReadAllText(BackupPath);

            using (IServiceScope serviceScope = applicationDomain.ServiceProvider.CreateScope())
            {
                GestaoPessoasDbContext databaseContext = serviceScope.ServiceProvider.GetRequiredService<GestaoPessoasDbContext>();
                databaseContext.Database.Migrate();
                databaseContext.Database.ExecuteSqlRaw(sql);
                databaseContext.SaveChanges();

            }
        }       
    }
}

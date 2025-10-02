using GestaoPessoas.Dtos;
using GestaoPessoas.Services;
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
            applicationDomain.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
            service = applicationDomain.ServiceProvider.GetRequiredService<IWorkerService>();

            var configuration = applicationDomain.ServiceProvider.GetRequiredService<IConfiguration>();
            string? _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (_connectionString == null) Assert.Inconclusive("Não foi possível obter a connection string de configuração.");
            string? BackupPath = configuration.GetValue<string>("PostGresWorkerService:FilePath");
            if (BackupPath == null) throw new Exception("Não foi possível obter o caminho do ficheiro de configuração.");

            string sql = File.ReadAllText(BackupPath);

            using var conn = new NpgsqlConnection(_connectionString);
            {
                conn.Open();
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }       
    }
}

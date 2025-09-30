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
        public WorkerServicePostGresTests()
        {
            applicationDomain = new TestApplicationDomain();
            applicationDomain.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
            service = applicationDomain.ServiceProvider.GetRequiredService<IWorkerService>();

            var configuration = applicationDomain.ServiceProvider.GetRequiredService<IConfiguration>();
            string? _connectionString = configuration.GetConnectionString("DefaultConnection");
            string? BackupPath = configuration.GetValue<string>("PostGresWorkerService:FilePath");
            if (BackupPath == null) throw new Exception("Não foi possível obter o caminho do ficheiro de configuração.");

            string sql = File.ReadAllText(BackupPath);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}

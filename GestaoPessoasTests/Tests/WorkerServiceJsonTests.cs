using GestaoPessoas.Dtos;
using GestaoPessoas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestaoPessoasTests.Tests
{
    [TestClass]
    [DoNotParallelize]
    public sealed class WorkerServiceJsonTests : WorkerServiceTestsBase
    {
        [TestInitialize]
        public void Initialize()
        {
            applicationDomain = new TestApplicationDomain();
            applicationDomain.Services.AddScoped<IWorkerService, WorkerServiceJsonFile>();
            service = applicationDomain.ServiceProvider.GetRequiredService<IWorkerService>();
            var configuration = applicationDomain.ServiceProvider.GetRequiredService<IConfiguration>();
            string? path = configuration.GetValue<string>("JsonWorkerService:FilePath");

            if (path == null) throw new Exception("Não foi possível obter o caminho do ficheiro de configuração.");

            File.Copy("BackupTestesInicial.json", path, true);
        }
    }
}

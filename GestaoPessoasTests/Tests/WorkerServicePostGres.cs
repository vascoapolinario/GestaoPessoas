using GestaoPessoas.Dtos;
using GestaoPessoas.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestaoPessoasTests.Tests
{
    [TestClass]
    public sealed class WorkerServicePostGresTests : WorkerServiceTestsBase
    {
        public WorkerServicePostGresTests()
        {
            applicationDomain = new TestApplicationDomain();
            applicationDomain.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
            service = applicationDomain.ServiceProvider.GetRequiredService<IWorkerService>();

            
        }
    }
}

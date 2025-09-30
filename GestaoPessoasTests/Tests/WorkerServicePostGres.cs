using GestaoPessoas.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPessoasTests.Tests
{
    [TestClass]
    public sealed class WorkerServicePostGresTests : WorkerServiceTestsBase
    {
        private TestApplicationDomain? applicationDomain;

        [TestMethod]
        public void TestMethod1()
        {
            applicationDomain = new TestApplicationDomain();
            applicationDomain.Services.AddScoped<IWorkerService, WorkerServicePostGres>();
            app
           // WorkerServicePostGres WorkerService;
        }
    }
}

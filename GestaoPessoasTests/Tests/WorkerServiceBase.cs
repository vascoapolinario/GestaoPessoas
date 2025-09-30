using GestaoPessoas.Dtos;
using GestaoPessoas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPessoasTests.Tests
{
    public class WorkerServiceTestsBase : IDisposable
    {

        protected TestApplicationDomain? applicationDomain;
        protected IWorkerService? service;

        public void Dispose()
        {
            applicationDomain?.Dispose();
        }


        [TestMethod]
        public void ListTest()
        {
            IEnumerable<Worker> workers = service!.GetAllWorkers();
            Assert.AreEqual(10, workers.Count());
            //var options = new JsonSerializerOptions { WriteIndented = true };
            //string json = JsonSerializer.Serialize(workers, options);
            //File.WriteAllText("C:\\Users\\User\\source\\repos\\GestaoPessoas1\\GestaoPessoasPasta\\GestaoPessoasTests\\Workers.json", json);
        }

        [TestMethod]
        public void TestGetById()
        {
            Worker comparisonworker = new Worker
            {
                Id = 3,
                Name = "teste",
                JobTitle = "testing",
                Email = "email@gmail.com",
                BirthDate = new DateOnly(2025, 01, 01)
            };

            Worker? realworker = service!.GetWorkerByIdIfExists(3);
            Assert.IsNotNull(realworker);
            Assert.AreEqual(comparisonworker, realworker);
        }

        public void TestAddWorker()
        {
            Worker newworker = new Worker
            {
                Name = "new worker",
                JobTitle = "new job",
                Email = "email@gmail.com",
                BirthDate = new DateOnly(2025, 01, 01)
            };
        }
    }
}

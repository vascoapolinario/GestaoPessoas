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
        }

        [TestMethod]
        public void TestGetById()
        {
            Worker comparisonworker = new Worker
            {
                Id = 13,
                Name = "TestUser13",
                JobTitle = "string",
                Email = "user@example.com",
                BirthDate = new DateOnly(2025, 09, 22)
            };

            Worker? realworker = service!.GetWorkerByIdIfExists(comparisonworker.Id);
            Assert.IsNotNull(realworker);
            Assert.AreEqual(comparisonworker, realworker);
        }

        [TestMethod]
        public void TestAddWorker()
        {
            Worker newworker = new Worker
            {
                Name = "new worker",
                JobTitle = "new job",
                Email = "email@gmail.com",
                BirthDate = new DateOnly(2025, 01, 01)
            };
            Worker addedworker = service!.AddWorker(newworker);
            Assert.IsNotNull(addedworker);
            newworker.Id = addedworker.Id;
            Assert.AreEqual(addedworker, newworker);
        }

        [TestMethod]

        public void TestUpdateWorker()
        {
            Worker updatedworker = new Worker
            {
                Id = 13,
                Name = "UpdatedName",
                JobTitle = "UpdatedJob",
                Email = "UpdatedEmail@gmail.com",
                BirthDate = new DateOnly(2024, 01, 01)
            };
            Worker? resultworker = service!.UpdateWorker(updatedworker);
            Assert.AreEqual(updatedworker, resultworker);
        }

        [TestMethod]
        public void TestRemoveWorker()
        {
            bool result = service!.RemoveWorker(13);
            Assert.IsTrue(result);
            Worker? worker = service.GetWorkerByIdIfExists(13);
            Assert.IsNull(worker);
        }
    }
}

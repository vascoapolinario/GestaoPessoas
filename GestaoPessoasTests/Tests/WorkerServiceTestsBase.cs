using GestaoPessoas.Dtos;
using GestaoPessoas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPessoasTests.Tests
{
    public abstract class WorkerServiceTestsBase : IDisposable
    {
        protected TestApplicationDomain? applicationDomain;
        protected IWorkerService? service;

        [TestCleanup]
        public void Cleanup()
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
                BirthDate = new DateOnly(2025, 09, 22),
                TimeZone = TimeZoneInfo.Utc
            };

            Worker? realworker = service!.GetWorkerByIdIfExists(comparisonworker.Id);
            Assert.IsNotNull(realworker);
            Assert.AreEqual(comparisonworker, realworker);
        }

        [TestMethod]
        public void TestGetByIdNotFound()
        {
            Worker? realworker = service!.GetWorkerByIdIfExists(0);
            Assert.IsNull(realworker);
        }

        [TestMethod]
        public void TestAddWorker()
        {
            Worker newworker = new Worker
            {
                Name = "new worker",
                JobTitle = "new job",
                Email = "email@gmail.com",
                BirthDate = new DateOnly(2025, 01, 01),
                TimeZone = TimeZoneInfo.Utc
            };
            Worker addedworker = service!.AddWorker(newworker);
            Assert.AreNotEqual(0, addedworker.Id);
            Assert.IsNotNull(addedworker);
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
                BirthDate = new DateOnly(2024, 01, 01),
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
            };
            Worker? realworker = service!.GetWorkerByIdIfExists(updatedworker.Id);
            Assert.AreNotEqual(updatedworker, realworker);
            Worker? resultworker = service!.UpdateWorker(updatedworker);
            Assert.AreEqual(updatedworker, resultworker);
        }

        [TestMethod]
        public void TestUpdateWorkerNotFound()
        {
            Worker updatedworker = new Worker
            {
                Id = 0,
                Name = "UpdatedName",
                JobTitle = "UpdatedJob",
                Email = "UpdatedEmail@gmail.com",
                BirthDate = new DateOnly(2024, 01, 01),
                TimeZone = TimeZoneInfo.Utc
            };
            Worker? resultworker = service!.UpdateWorker(updatedworker);
            Assert.IsNull(resultworker);
        }

        [TestMethod]
        public void TestRemoveWorker()
        {
            bool result = service!.RemoveWorker(13);
            Assert.IsTrue(result);
            Worker? worker = service.GetWorkerByIdIfExists(13);
            Assert.IsNull(worker);
        }

        [TestMethod]
        public void TestRemoveWorkerNotFound()
        {
            Assert.IsTrue(service.GetWorkerByIdIfExists(0) == null);
            bool result = service!.RemoveWorker(0);
            Assert.IsFalse(result);
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}

using GestaoPessoas.Dtos;
using GestaoPessoas.Services;

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
                TimeZone = TimeZoneInfo.Utc,
                PortalDasFinancasUserName = "user13",
                PortalDasFinancasUserPassword = "password"
            };

            Worker? realworker = service!.GetWorkerByIdIfExists(comparisonworker.Id);
            Assert.IsNotNull(realworker);
            Assert.IsTrue(WorkersAreEqual(comparisonworker, realworker!));
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
                TimeZone = TimeZoneInfo.Utc,
                PortalDasFinancasUserName = "newusername",
                PortalDasFinancasUserPassword = "newpassword"
            };
            Worker addedworker = service!.AddWorker(newworker);
            Assert.AreNotEqual(0, addedworker.Id);
            Assert.IsNotNull(addedworker);
            Assert.IsTrue(WorkersAreEqual(newworker, addedworker));
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
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles"),
                PortalDasFinancasUserName = "UpdatedUserName",
                PortalDasFinancasUserPassword = "UpdatedPassword"
            };
            Worker? realworker = service!.GetWorkerByIdIfExists(updatedworker.Id);
            Assert.IsNotNull(realworker);
            Assert.IsFalse(WorkersAreEqual(updatedworker, realworker!));
            Worker? resultworker = service!.UpdateWorker(updatedworker);
            Assert.IsNotNull(resultworker);
            Assert.IsTrue(WorkersAreEqual(updatedworker, resultworker!));
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
                TimeZone = TimeZoneInfo.Utc,
                PortalDasFinancasUserName = "UpdatedUserName",
                PortalDasFinancasUserPassword = "UpdatedPassword"
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
            Assert.IsTrue(service!.GetWorkerByIdIfExists(0) == null);
            bool result = service!.RemoveWorker(0);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestClearSensitiveData()
        {
            Worker newworker = new Worker
            {
                Name = "new worker",
                JobTitle = "new job",
                Email = "email@gmail.com",
                BirthDate = new DateOnly(2025, 01, 01),
                TimeZone = TimeZoneInfo.Utc,
                PortalDasFinancasUserName = "newusername",
                PortalDasFinancasUserPassword = "newpassword"
            };
            service!.ClearSensitiveData(newworker);
            Assert.IsNotNull(newworker);
            Assert.IsNull(newworker.PortalDasFinancasUserPassword);
            Assert.IsNotNull(newworker.Name);
            Assert.IsNotNull(newworker.JobTitle);
            Assert.IsNotNull(newworker.Email);
            Assert.IsNotNull(newworker.BirthDate);
            Assert.IsNotNull(newworker.TimeZone);
            Assert.IsNotNull(newworker.PortalDasFinancasUserName);
        }

        public void Dispose()
        {
            Cleanup();
        }

        private bool WorkersAreEqual(Worker worker1, Worker worker2)
        {
            return
                   worker1.Name == worker2.Name &&
                   worker1.JobTitle == worker2.JobTitle &&
                   worker1.Email == worker2.Email &&
                   worker1.BirthDate == worker2.BirthDate &&
                   worker1.TimeZone.Id == worker2.TimeZone.Id &&
                   worker1.PortalDasFinancasUserName == worker2.PortalDasFinancasUserName &&
                   worker1.PortalDasFinancasUserPassword == worker2.PortalDasFinancasUserPassword;
        }
    }
}

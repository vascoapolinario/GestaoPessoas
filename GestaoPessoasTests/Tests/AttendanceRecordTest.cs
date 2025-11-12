using GestaoPessoas.DataSources;
using GestaoPessoas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dtos = GestaoPessoas.Dtos;

namespace GestaoPessoasTests.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class AttendanceRecordTest
    {
        private TestApplicationDomain? applicationDomain;
        private AttendanceRecordService service;

        [TestInitialize]
        public void Initialize()
        {
            applicationDomain = new TestApplicationDomain();
            string? _connectionString = applicationDomain.configuration.GetConnectionString("DefaultConnection");
            if (_connectionString == null) Assert.Inconclusive("Não foi possível obter a connection string de configuração.");
            applicationDomain.Services.AddDbContext<GestaoPessoasDbContext>(o => o.UseNpgsql(_connectionString));
            applicationDomain.Services.AddScoped<AttendanceRecordService>();
            service = applicationDomain.ServiceProvider.GetRequiredService<AttendanceRecordService>();

            string? BackupPath = applicationDomain.configuration.GetValue<string>("AttendanceRecordService:FilePath");
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

        [TestCleanup]
        public void Cleanup()
        {
            applicationDomain?.Dispose();
        }

        [TestMethod]
        public void ListAttendanceRecordsTest()
        {
            var records = service.GetAllAttendanceRecords();
            Assert.AreEqual(5, records.Count());
        }

        [TestMethod]
        public void GetAttendanceRecordTest()
        {
            Dtos.AttendanceRecord expectedRecord = new Dtos.AttendanceRecord
            {
                Id = 3,
                WorkerId = 2,
                WorkerName = "teste",
                EventType = "CheckIn",
                EventDate = new DateTime(2025, 1, 2, 9, 0, 0, DateTimeKind.Utc)
            };

            var actualRecord = service.GetAttendanceRecordByIdIfExists(3);
            Assert.IsNotNull(actualRecord);
            Assert.IsTrue(AreAttendanceRecordsEqual(expectedRecord, actualRecord!));
        }

        [TestMethod]
        public void GetAttendanceRecordNotFoundTest()
        {
            var record = service.GetAttendanceRecordByIdIfExists(999);
            Assert.IsNull(record);
        }

        [TestMethod]
        public void AddAttendanceRecordTest()
        {
            Dtos.AttendanceRecord newRecord = new Dtos.AttendanceRecord
            {
                WorkerId = 3,
                WorkerName = "Vasco Jacinto Apolinario",
                EventType = "CheckOut",
                EventDate = new DateTime(2025, 1, 2, 17, 0, 0, DateTimeKind.Utc)
            };
            var createdRecord = service.AddAttendanceRecord(newRecord);

            Assert.IsNotNull(createdRecord);
            Assert.IsTrue(createdRecord.Id > 0);
            Assert.IsTrue(AreAttendanceRecordsEqual(newRecord , createdRecord));
        }

        [TestMethod]
        public void AddAttendanceRecordForNonExistentWorkerTest()
        {
            Dtos.AttendanceRecord newRecord = new Dtos.AttendanceRecord
            {
                WorkerId = 999,
                WorkerName = "Non Existent Worker",
                EventType = "CheckIn",
                EventDate = new DateTime(2025, 1, 2, 9, 0, 0, DateTimeKind.Utc)
            };

            var createdRecord = service.AddAttendanceRecord(newRecord);
            Assert.IsNull(createdRecord);
        }

        [TestMethod]
        public void RemoveAttendanceRecordTest()
        {
            var recordToDelete = service.GetAttendanceRecordByIdIfExists(4);
            Assert.IsNotNull(recordToDelete);

            service.RemoveAttendanceRecord(4);
            var deletedRecord = service.GetAttendanceRecordByIdIfExists(4);
            Assert.IsNull(deletedRecord);
        }

        [TestMethod]
        public void RemoveNonExistentAttendanceRecordTest()
        {
            var recordToDelete = service.GetAttendanceRecordByIdIfExists(999);
            Assert.IsNull(recordToDelete);

            service.RemoveAttendanceRecord(999);
            var deletedRecord = service.GetAttendanceRecordByIdIfExists(999);
            Assert.IsNull(deletedRecord);
        }

        [TestMethod]
        public void UpdateAttendanceRecordTest()
        {
            var originalRecord = service.GetAttendanceRecordByIdIfExists(5);
            var recordToUpdate = service.GetAttendanceRecordByIdIfExists(5);
            Assert.IsNotNull(recordToUpdate);

            recordToUpdate!.EventType = "CheckOut";
            recordToUpdate.EventDate = new DateTime(2025, 1, 2, 18, 0, 0, DateTimeKind.Utc);
            var updatedRecord = service.UpdateAttendanceRecord(recordToUpdate);

            Assert.IsNotNull(updatedRecord);
            Assert.IsTrue(AreAttendanceRecordsEqual(recordToUpdate, updatedRecord!));
            Assert.IsFalse(AreAttendanceRecordsEqual(originalRecord, updatedRecord));
        }


        private bool AreAttendanceRecordsEqual(Dtos.AttendanceRecord expected, Dtos.AttendanceRecord actual)
        {
            return 
                   expected.WorkerId == actual.WorkerId &&
                   expected.WorkerName == actual.WorkerName &&
                   expected.EventType == actual.EventType &&
                   expected.EventDate == actual.EventDate;
        }

    }
}
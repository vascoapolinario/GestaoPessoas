using GestaoPessoas.Converters;
using GestaoPessoas.Dtos;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace GestaoPessoas.Services
{
    public class WorkerServiceJsonFile : IWorkerService
    {
        private string filepath;
        private static readonly object _fileLock = new object();
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new TimeZoneInfoJsonConverter() }
        };
        private ICryptoService cryptoService;

        public WorkerServiceJsonFile(IConfiguration configuration, ICryptoService cryptoService)
        {
            filepath = configuration.GetValue<string>("JsonWorkerService:FilePath") ?? "./workers.json";
            this.cryptoService = cryptoService;
        }

        private IEnumerable<Worker> LoadWorkersFromJson()
        {
            string json;
            lock (_fileLock)
            {
                if (!File.Exists(filepath))
                    return Enumerable.Empty<Worker>();

                json = File.ReadAllText(filepath);
            }
            if (string.IsNullOrWhiteSpace(json))
                return Enumerable.Empty<Worker>();

            var doc = JsonDocument.Parse(json);

            var workers = JsonSerializer.Deserialize<List<Worker>>(json, _jsonOptions) ?? new List<Worker>();
            return workers.OrderBy(worker => worker.Name);
        }

        private void SaveWorkersToJson(IEnumerable<Worker> workers)
        {
            string json = JsonSerializer.Serialize(workers, _jsonOptions);
            lock (_fileLock)
            {
                File.WriteAllText(filepath, json);
            }
        }

        public Worker AddWorker(Worker worker)
        {
            var workers = LoadWorkersFromJson()?.ToList() ?? new List<Worker>();
            worker.Id = workers.Count > 0 ? workers.Max(w => w.Id) + 1 : 1;
            worker.PortalDasFinancasUserPassword = cryptoService.Encrypt(worker.PortalDasFinancasUserPassword)!;
            workers.Add(worker);
            SaveWorkersToJson(workers);
            return worker;
        }

        public bool RemoveWorker(int id)
        {
            var workers = LoadWorkersFromJson()?.ToList() ?? new List<Worker>();
            var workerToRemove = workers.FirstOrDefault(w => w.Id == id);
            if (workerToRemove != null)
            {
                workers.Remove(workerToRemove);
                SaveWorkersToJson(workers);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Worker> GetAllWorkers()
        {
            IEnumerable<Worker> workers = LoadWorkersFromJson();
            foreach (var worker in workers) {
                if (worker.PortalDasFinancasUserPassword != String.Empty)
                    worker.PortalDasFinancasUserPassword = cryptoService.Decrypt(worker.PortalDasFinancasUserPassword)!;
            }
            return workers;
        }

        public Worker? GetWorkerByIdIfExists(int id)
        {
            var workers = LoadWorkersFromJson();
            Worker? selected = workers?.FirstOrDefault(w => w.Id == id);
            if (selected != null && selected.PortalDasFinancasUserPassword != String.Empty)
            {
                selected.PortalDasFinancasUserPassword = cryptoService.Decrypt(selected.PortalDasFinancasUserPassword)!;
            }
            return selected;
        }

        public Worker? UpdateWorker(Worker worker)
        {
            var workers = LoadWorkersFromJson()?.ToList() ?? new List<Worker>();
            var existingWorker = workers.FirstOrDefault(w => w.Id == worker.Id);
            if (existingWorker != null)
            {
                existingWorker.Name = worker.Name;
                existingWorker.Email = worker.Email;
                existingWorker.JobTitle = worker.JobTitle;
                existingWorker.BirthDate = worker.BirthDate;
                existingWorker.TimeZone = worker.TimeZone;
                existingWorker.PortalDasFinancasUserName = worker.PortalDasFinancasUserName;
                existingWorker.PortalDasFinancasUserPassword = cryptoService.Encrypt(worker.PortalDasFinancasUserPassword)!;
                SaveWorkersToJson(workers);
                existingWorker.PortalDasFinancasUserPassword = cryptoService.Decrypt(existingWorker.PortalDasFinancasUserPassword)!;
                return existingWorker;
            }
            else
            {
                return null;
            }
        }

        public Worker NonSensitiveWorker(Worker worker)
        {
            worker.PortalDasFinancasUserPassword = String.Empty;
            return worker;
        }
    }
}

using System.Runtime.CompilerServices;
using System.Text.Json;
using GestaoPessoas.Dtos;

namespace GestaoPessoas.Services
{
    public class WorkerServiceJsonFile : IWorkerService
    {
        private string filepath;

        public WorkerServiceJsonFile(IConfiguration configuration)
        {
            filepath = configuration.GetValue<string>("JsonWorkerService:FilePath") ?? "./workers.json";
        }

        private IEnumerable<Worker> LoadWorkersFromJson()
        {
            if (!File.Exists(filepath))
                return Enumerable.Empty<Worker>();

            string json = File.ReadAllText(filepath);
            if (string.IsNullOrWhiteSpace(json))
                return Enumerable.Empty<Worker>();

            using var doc = JsonDocument.Parse(json);
            var workers = new List<Worker>();

            workers = JsonSerializer.Deserialize<List<Worker>>(json) ?? new List<Worker>();

            return workers;
        }

        public Worker AddWorker(Worker worker)
        {
            var workers = LoadWorkersFromJson()?.ToList() ?? new List<Worker>();
            worker.Id = workers.Count > 0 ? workers.Max(w => w.Id) + 1 : 1;
            workers.Add(worker);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(workers, options);
            File.WriteAllText(filepath, json);
            return worker;
        }

        public bool RemoveWorker(int id)
        {
            var workers = LoadWorkersFromJson()?.ToList() ?? new List<Worker>();
            var workerToRemove = workers.FirstOrDefault(w => w.Id == id);
            if (workerToRemove != null)
            {
                workers.Remove(workerToRemove);
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(workers, options);
                File.WriteAllText(filepath, json);
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
            return workers;
        }

        public Worker? GetWorkerByIdIfExists(int id)
        {
            var workers = LoadWorkersFromJson();
            Worker? selected = workers?.FirstOrDefault(w => w.Id == id);
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
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(workers, options);
                File.WriteAllText(filepath, json);
                return existingWorker;
            }
            else
            {
                throw new KeyNotFoundException($"Worker with ID {worker.Id} not found.");
            }
        }
    }
}

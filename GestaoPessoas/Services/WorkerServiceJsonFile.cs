using System.Runtime.CompilerServices;
using System.Text.Json;
using GestaoPessoas.Dtos;

namespace GestaoPessoas.Services
{
    public class WorkerServiceJsonFile : IWorkerService
    {
        private string filepath;
        private static readonly object _fileLock = new object();

        public WorkerServiceJsonFile(IConfiguration configuration)
        {
            filepath = configuration.GetValue<string>("JsonWorkerService:FilePath") ?? "./workers.json";
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

            var workers = JsonSerializer.Deserialize<List<Worker>>(json) ?? new List<Worker>();
            return workers.OrderBy(worker => worker.Name);

            return workers;
            
        }

        private void SaveWorkersToJson(IEnumerable<Worker> workers)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(workers, options);
            lock (_fileLock)
            {
                File.WriteAllText(filepath, json);
            }
        }

        public Worker AddWorker(Worker worker)
        {
            var workers = LoadWorkersFromJson()?.ToList() ?? new List<Worker>();
            worker.Id = workers.Count > 0 ? workers.Max(w => w.Id) + 1 : 1;
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
                SaveWorkersToJson(workers);
                return existingWorker;
            }
            else
            {
                return null;
            }
        }

    }
}

using GestaoPessoas.Dtos;

namespace GestaoPessoas.Services
{
    public interface IWorkerService
    {
        IEnumerable<Worker> GetAllWorkers();
        Worker? GetWorkerByIdIfExists(int id);
        Worker AddWorker(Worker worker);
        bool RemoveWorker(int id);
        Worker? UpdateWorker(Worker worker);
    }
}
using GestaoPessoas.Services;
using Microsoft.AspNetCore.Mvc;
using GestaoPessoas.Dtos;
using System.Threading.Tasks.Dataflow;
using System.ComponentModel.DataAnnotations;

namespace GestaoPessoas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkersController : ControllerBase
    {
        //private static WorkerServiceJsonFile WorkerService = new WorkerServiceJsonFile("./workers.json");

        private readonly IWorkerService WorkerService;

        private readonly ILogger<WorkersController> _logger;

        public WorkersController(ILogger<WorkersController> logger, IWorkerService workerService)
        {
            _logger = logger;
            WorkerService = workerService;
        }

        [HttpGet(Name = "Workers")]
        public ActionResult<IEnumerable<Worker>> GetWorkers()
        {

                var worker = WorkerService.GetAllWorkers();
                return Ok(worker);

        }

        [HttpGet("{id}", Name = "Worker")]
        public ActionResult<Worker> GetWorker(int id)
        {
            var worker = WorkerService.GetWorkerByIdIfExists(id);
            if (worker is null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

        [HttpPost(Name = "AddWorker")]
        public ActionResult<Worker> AddWorker(Worker newworker)
        {
            Worker? createdWorker = WorkerService.AddWorker(newworker);
            return CreatedAtAction(nameof(GetWorker), new {id = createdWorker.Id}, createdWorker);
        }

        [HttpDelete("{id}", Name = "DeleteWorker")]
        public ActionResult DeleteWorker(int id)
        {
            if (WorkerService.RemoveWorker(id))
            {
                return Ok();
            }
            else
            {
                return NotFound($"Worker with ID {id} not found.");
            }
        }

        [HttpPut(Name = "UpdateWorker")]
        public ActionResult<Worker> UpdateWorker(Worker updatedWorker)
        {

            var worker = WorkerService.UpdateWorker(updatedWorker);
              
            if (worker is null)
            {
                return NotFound($"Worker with ID {updatedWorker.Id} not found.");
            }
            return Ok(worker);
        }
    }
}

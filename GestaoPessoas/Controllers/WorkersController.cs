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

        /// <summary>
        /// Retorna todos os trabalhadores.
        /// </summary>
        /// <returns>Lista de trabalhadores</returns>
        [HttpGet(Name = "Workers")]
        [ProducesResponseType(typeof(IEnumerable<Worker>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Worker>> GetWorkers()
        {

                var worker = WorkerService.GetAllWorkers();
                return Ok(worker);

        }

        /// <summary>
        /// Retorna um trabalhador através do seu ID.
        /// </summary>
        /// <param name="id">Id do trabalhador.</param>
        /// <returns>O trabalhador caso seja encontrado, caso contrário resposta NotFound</returns>
        [HttpGet("{id}", Name = "Worker")]
        [ProducesResponseType(typeof(Worker), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Worker> GetWorker(int id)
        {
            var worker = WorkerService.GetWorkerByIdIfExists(id);
            if (worker is null)
            {
                return NotFound();
            }
            return Ok(worker);
        }

        /// <summary>
        /// Addiciona um novo trabalhador.
        /// </summary>
        /// <param name="newworker">O novo trabalhador a adicionar</param>
        /// <returns>O worker e o seu url juntamente com resposta 201 ou 400 caso o pedido contenha dados invalidos</returns>
        [HttpPost(Name = "AddWorker")]
        [ProducesResponseType(typeof(Worker), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<Worker> AddWorker([FromBody] Worker newworker)
        {
            Worker? createdWorker = WorkerService.AddWorker(newworker);
            return CreatedAtAction(nameof(GetWorker), new {id = createdWorker.Id}, createdWorker);
        }


        /// <summary>
        /// Apaga um trabalhador através do seu ID.
        /// </summary>
        /// <param name="id">Id do trabalhador a apagar</param>
        /// <returns>Resposta do tipo 200 para sucesso e 404 caso não seja encontrado o trabalhador</returns>
        [HttpDelete("{id}", Name = "DeleteWorker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Atualiza um trabalhador.
        /// </summary>
        /// <param name="updatedWorker">Trabalhador a atualizar</param>
        /// <returns>Resposta do tipo 200 com o trabalhador se encontrado, 400 caso o pedido contenha dados invalidos ou 404 se o trabalhador não for encontrado.</returns>
        [HttpPut(Name = "UpdateWorker")]
        [ProducesResponseType(typeof(Worker), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<Worker> UpdateWorker([FromBody] Worker updatedWorker)
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

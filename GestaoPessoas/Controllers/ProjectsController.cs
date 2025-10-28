using GestaoPessoas.Services;
using Microsoft.AspNetCore.Mvc;
using DtoEntities = GestaoPessoas.Dtos;

namespace GestaoPessoas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        //private static WorkerServiceJsonFile WorkerService = new WorkerServiceJsonFile("./workers.json");

        private readonly ProjectService ProjectService;

        private readonly ILogger<WorkersController> _logger;

        public ProjectsController(ILogger<WorkersController> logger, ProjectService projectService)
        {
            _logger = logger;
            ProjectService = projectService;
        }

        /// <summary>
        /// Retorna todos os Projetos.
        /// </summary>
        /// <returns>Lista de Projetos</returns>
        [HttpGet(Name = "Projects")]
        [ProducesResponseType(typeof(IEnumerable<DtoEntities.Project>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DtoEntities.Project>> GetProjects()
        {
            IEnumerable<DtoEntities.Project> Projects = ProjectService.GetAllProjects();

            return Ok(Projects);
        }

        /// <summary>
        /// Retorna um Projeto através do seu ID.
        /// </summary>
        /// <param name="id">Id do Projeto.</param>
        /// <returns>O Projeto caso seja encontrado, caso contrário resposta NotFound</returns>
        [HttpGet("{id}", Name = "Project")]
        [ProducesResponseType(typeof(DtoEntities.Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoEntities.Project> GetProject(int id)
        {
            var Project = ProjectService.GetProjectByIdIfExists(id);
            if (Project is null)
            {
                return NotFound();
            }
            return Ok(Project);
        }

        /// <summary>
        /// Addiciona um novo projeto.
        /// </summary>
        /// <param name="newproject">O novo projeto a adicionar</param>
        /// <returns>O projeto e o seu url juntamente com resposta 201 ou 400 caso o pedido contenha dados invalidos</returns>
        [HttpPost(Name = "AddProject")]
        [ProducesResponseType(typeof(DtoEntities.Project), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<DtoEntities.Project> AddProject([FromBody] DtoEntities.Project newproject)
        {
            DtoEntities.Project? createdProject = ProjectService.AddProject(newproject);
            if (createdProject is null)
            {
                return BadRequest("Projected with name '" + newproject.Name + "' already exists.");
            }

            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
        }


        /// <summary>
        /// Apaga um projeto através do seu ID.
        /// </summary>
        /// <param name="id">Id do projeto a apagar</param>
        /// <returns>Resposta do tipo 200 para sucesso e 404 caso não seja encontrado o projeto</returns>
        [HttpDelete("{id}", Name = "DeleteProject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteProject(int id)
        {
            if (ProjectService.RemoveProject(id))
            {
                return Ok();
            }
            else
            {
                return NotFound($"Worker with ID {id} not found.");
            }
        }

        /// <summary>
        /// Atualiza um projeto.
        /// </summary>
        /// <param name="worker">Projeto a atualizar</param>
        /// <returns>Resposta do tipo 200 com o projeto se encontrado, 400 caso o pedido contenha dados invalidos ou 404 se o projeto não for encontrado.</returns>
        [HttpPut(Name = "UpdateProject")]
        [ProducesResponseType(typeof(DtoEntities.Project), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public ActionResult<DtoEntities.Project> UpdateProject([FromBody] DtoEntities.Project worker)
        {

            var updatedProject = ProjectService.UpdateProject(worker);

            if (updatedProject is null)
            {
                return NotFound($"Worker with ID {worker.Id} not found. Or new project name is already used by another project");
            }
            return Ok(updatedProject);
        }

        /// <summary>
        /// Adiciona um trabalhador a um projeto.
        /// </summary>
        /// <param name="projectId">Id do projeto.</param>
        /// <param name="workerId">Id do trabalhador.</param>
        /// <returns>Resposta do tipo 200 com o projeto, resposta do tipo 400 se o trabalhador ou projeto não foram encontrados</returns>
        [HttpPost("AddWorkerToProject", Name = "AddWorkerToProject")]
        [ProducesResponseType(typeof(DtoEntities.Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoEntities.Project> AddWorkerToProject([FromQuery] int projectId, [FromQuery] int workerId)
        {
            if (ProjectService.IsWorkerInProject(projectId, workerId))
            {
                return BadRequest($"Worker with ID {workerId} is already in Project with ID {projectId}.");
            }
            var updatedProject = ProjectService.AddWorkerToProject(projectId, workerId);
            if (updatedProject is null)
            {
                return NotFound($"Project with ID {projectId} or Worker with ID {workerId} not found.");
            }
            return Ok(updatedProject);
        }

        /// <summary>
        /// Remove um trabalhador de um projeto.
        /// </summary>
        /// <param name="projectId">Id do projeto.</param>
        /// <param name="workerId">Id do trabalhador.</param>
        /// <returns>Resposta do tipo 200 com o projeto, resposta do tipo 404 se o trabalhador ou projeto não foram encontrados</returns>
        [HttpPost("RemoveWorkerFromProject", Name = "RemoveWorkerFromProject")]
        [ProducesResponseType(typeof(DtoEntities.Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoEntities.Project> RemoveWorkerFromProject([FromQuery] int projectId, [FromQuery] int workerId)
        {
            if (!ProjectService.IsWorkerInProject(projectId, workerId))
            {
                return BadRequest($"Worker with ID {workerId} is not in Project with ID {projectId}.");
            }
            var updatedProject = ProjectService.RemoveWorkerFromProject(projectId, workerId);
            if (updatedProject is null)
            {
                return NotFound($"Project with ID {projectId} or Worker with ID {workerId} not found.");
            }
            return Ok(updatedProject);
        }

    }
}

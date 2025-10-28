using GestaoPessoas.Services;
using Microsoft.AspNetCore.Mvc;
using DtoEntities = GestaoPessoas.Dtos;

namespace GestaoPessoas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttendanceRecordsController : ControllerBase
    {
        private readonly AttendanceRecordService AttendanceRecordService;

        private readonly ILogger<WorkersController> _logger;

        public AttendanceRecordsController(ILogger<WorkersController> logger, AttendanceRecordService attendanceRecordService)
        {
            _logger = logger;
            AttendanceRecordService = attendanceRecordService;
        }

        /// <summary>
        /// Retorna todos os registos de presenças.
        /// </summary>
        /// <returns>Lista de registos de presenças</returns>
        [HttpGet(Name = "AttendanceRecords")]
        [ProducesResponseType(typeof(IEnumerable<DtoEntities.AttendanceRecord>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DtoEntities.AttendanceRecord>> GetAttendanceRecords()
        {
            IEnumerable<DtoEntities.AttendanceRecord> attendanceRecords = AttendanceRecordService.GetAllAttendanceRecords();
            return Ok(attendanceRecords);

        }

        /// <summary>
        /// Retorna um registo de presenças através do seu ID.
        /// </summary>
        /// <returns>O registo de presenças caso seja encontrado, caso contrário resposta NotFound</returns>
        [HttpGet("{id}", Name = "AttendanceRecord")]
        [ProducesResponseType(typeof(DtoEntities.AttendanceRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoEntities.AttendanceRecord> GetAttendanceRecord(int id)
        {
            var attendanceRecord = AttendanceRecordService.GetAttendanceRecordByIdIfExists(id);
            if (attendanceRecord is null)
            {
                return NotFound();
            }
            return Ok(attendanceRecord);
        }


        /// <summary>
        /// Adiciona um novo registo de presenças.
        /// </summary>
        /// <param name="newAttendanceRecord">O novo registo de presenças a adicionar</param>
        /// <returns>O registo de presenças e o seu url juntamente com resposta 201 ou 400 caso o pedido contenha dados invalidos ou 404 se o Worker não existir</returns>
        [HttpPost(Name = "AddAttendanceRecord")]
        [ProducesResponseType(typeof(DtoEntities.AttendanceRecord), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoEntities.AttendanceRecord> AddAttendanceRecord(DtoEntities.AttendanceRecord newAttendanceRecord)
        {
            var addedRecord = AttendanceRecordService.AddAttendanceRecord(newAttendanceRecord);
            if (addedRecord is null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAttendanceRecord), new { id = addedRecord.Id }, addedRecord);
        }

        /// <summary>
        /// Apaga um registo de presenças através do seu ID.
        /// </summary>
        /// <param name="id">Id do registo de presenças.</param>
        /// <returns>Resposta 200 caso o registo seja apagado com sucesso, resposta 404 caso o registo não seja encontrado.</returns>
        [HttpDelete("{id}", Name = "DeleteAttendanceRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteAttendanceRecord(int id)
        {
            bool removed = AttendanceRecordService.RemoveAttendanceRecord(id);
            if (!removed)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Atualiza um registo de presenças
        /// </summary>
        /// <param name="AttendanceRecordToUpdate">Registo de presenças com os dados a atualizar</param>
        /// <returns>O registo de presenças atualizado com resposta 200 se sucesso,400 caso haja dados invalidos e 404 caso o registo não seja encontrado</returns>
        [HttpPut(Name = "UpdateAttendanceRecord")]
        [ProducesResponseType(typeof(DtoEntities.AttendanceRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DtoEntities.AttendanceRecord> UpdateAttendanceRecord(DtoEntities.AttendanceRecord AttendanceRecordToUpdate)
        {
            var updatedRecord = AttendanceRecordService.UpdateAttendanceRecord(AttendanceRecordToUpdate);
            if (updatedRecord is null)
            {
                return NotFound();
            }
            return Ok(updatedRecord);
        }
    }
}

namespace GestaoPessoas.Services
{
    public class AttendanceRecordService
    {
        private readonly DataSources.GestaoPessoasDbContext _dbContext;

        public AttendanceRecordService(DataSources.GestaoPessoasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Dtos.AttendanceRecord> GetAllAttendanceRecords()
        {
            var records = from r in _dbContext.AttendanceRecords
                          orderby r.EventDate descending
                          select new Dtos.AttendanceRecord
                          {
                              Id = r.Id,
                              WorkerId = r.WorkerId,
                              WorkerName = r.Worker.Name,
                              EventType = r.EventType.ToString(),
                              EventDate = r.EventDate
                          };
            return records.ToList();
        }

        public Dtos.AttendanceRecord? GetAttendanceRecordByIdIfExists(int id)
        {
            var record = (from r in _dbContext.AttendanceRecords
                          where r.Id == id
                          select new Dtos.AttendanceRecord
                          {
                              Id = r.Id,
                              WorkerId = r.WorkerId,
                              WorkerName = r.Worker.Name,
                              EventType = r.EventType.ToString(),
                              EventDate = r.EventDate
                          }).FirstOrDefault();
            return record;
        }

        public Dtos.AttendanceRecord? AddAttendanceRecord(Dtos.AttendanceRecord record)
        {
            var worker = _dbContext.Workers.FirstOrDefault(w => w.Id == record.WorkerId);
            if (worker == null)
            {
                return null;
            }
            var newRecord = new Entities.AttendanceRecord
            {
                WorkerId = worker.Id,
                Worker = worker,
                EventType = Enum.TryParse<Enums.AttendanceEventType>(record.EventType, out var eventType) ? eventType : Enums.AttendanceEventType.Undefined,
                EventDate = record.EventDate
            };
            _dbContext.AttendanceRecords.Add(newRecord);
            _dbContext.SaveChanges();
            record.Id = newRecord.Id;
            return GetAttendanceRecordByIdIfExists(newRecord.Id)!;
        }

        public bool RemoveAttendanceRecord(int id)
        {
            var recordToRemove = _dbContext.AttendanceRecords.FirstOrDefault(r => r.Id == id);
            if (recordToRemove != null)
            {
                _dbContext.AttendanceRecords.Remove(recordToRemove);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Dtos.AttendanceRecord? UpdateAttendanceRecord(Dtos.AttendanceRecord record)
        {
            var recordToUpdate = _dbContext.AttendanceRecords.FirstOrDefault(r => r.Id == record.Id);
            if (recordToUpdate == null)
            {
                return null;
            }
            recordToUpdate.EventType = Enum.TryParse<Enums.AttendanceEventType>(record.EventType, out var eventType) ? eventType : Enums.AttendanceEventType.Undefined;
            recordToUpdate.EventDate = record.EventDate;
            _dbContext.SaveChanges();
            return GetAttendanceRecordByIdIfExists(recordToUpdate.Id);
        }
    }
}

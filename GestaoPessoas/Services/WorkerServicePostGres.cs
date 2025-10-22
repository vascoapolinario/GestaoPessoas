using GestaoPessoas.DataSources;
using Dto = GestaoPessoas.Dtos;
using Ef = GestaoPessoas.Entities;

namespace GestaoPessoas.Services
{
    public class WorkerServicePostGres : IWorkerService
    {
        private readonly GestaoPessoasDbContext dbContext;
        private ICryptoService cryptoService;

        public WorkerServicePostGres(GestaoPessoasDbContext dbContext, ICryptoService cryptoService)
        {
            this.dbContext = dbContext;
            this.cryptoService = cryptoService;
        }

        public Dto.Worker AddWorker(Dto.Worker worker)
        {

            Ef.Worker new_worker = new Ef.Worker
            {
                Name = worker.Name,
                JobTitle = worker.JobTitle,
                Email = worker.Email,
                BirthDate = worker.BirthDate,
                TimeZone = worker.TimeZone.Id,
                PortalDasFinancasUserName = worker.PortalDasFinancasUserName,
                PortalDasFinancasUserPassword = cryptoService.Encrypt(worker.PortalDasFinancasUserPassword)
            };
            dbContext.Workers.Add(new_worker);
            dbContext.SaveChanges();
            return GetWorkerByIdIfExists(new_worker.Id)!;

        }

        public IEnumerable<Dto.Worker> GetAllWorkers()
        {
            List<Dto.Worker> workers = (from w in dbContext.Workers
                                        orderby w.Name
                                        select new Dto.Worker
                                        {
                                            Id = w.Id,
                                            Name = w.Name,
                                            JobTitle = w.JobTitle,
                                            Email = w.Email,
                                            BirthDate = w.BirthDate,
                                            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(w.TimeZone),
                                            PortalDasFinancasUserName = w.PortalDasFinancasUserName,
                                            PortalDasFinancasUserPassword = cryptoService.Decrypt(w.PortalDasFinancasUserPassword)
                                        }).ToList();
            return workers;
        }

        public Dto.Worker? GetWorkerByIdIfExists(int id)
        {
            Dto.Worker? worker = (from w in dbContext.Workers
                                  where w.Id == id
                                  select new Dto.Worker
                                  {
                                      Id = w.Id,
                                      Name = w.Name,
                                      JobTitle = w.JobTitle,
                                      Email = w.Email,
                                      BirthDate = w.BirthDate,
                                      TimeZone = TimeZoneInfo.FindSystemTimeZoneById(w.TimeZone),
                                      PortalDasFinancasUserName = w.PortalDasFinancasUserName,
                                      PortalDasFinancasUserPassword = cryptoService.Decrypt(w.PortalDasFinancasUserPassword)

                                  }).SingleOrDefault();
            return worker;
        }

        public bool RemoveWorker(int id)
        {
            Ef.Worker? worker = (from w in dbContext.Workers
                                 where w.Id == id
                                 select w).SingleOrDefault();

            if (worker == null) return false;
            dbContext.Workers.Remove(worker);
            dbContext.SaveChanges();
            return true;

        }

        public Dto.Worker? UpdateWorker(Dto.Worker worker)
        {
            var current_worker = (from w in dbContext.Workers
                                  where w.Id == worker.Id
                                  select w).SingleOrDefault();

            if (current_worker == null) return null;
            current_worker.Name = worker.Name;
            current_worker.JobTitle = worker.JobTitle;
            current_worker.Email = worker.Email;
            current_worker.BirthDate = worker.BirthDate;
            current_worker.TimeZone = worker.TimeZone.Id;
            current_worker.PortalDasFinancasUserName = worker.PortalDasFinancasUserName;
            current_worker.PortalDasFinancasUserPassword = cryptoService.Encrypt(worker.PortalDasFinancasUserPassword);
            dbContext.SaveChanges();
            return GetWorkerByIdIfExists(worker.Id);
        }

        public void ClearSensitiveData(params Dto.Worker[] workers)
        {
            foreach (var worker in workers) worker.PortalDasFinancasUserPassword = null;
        }
    }
}

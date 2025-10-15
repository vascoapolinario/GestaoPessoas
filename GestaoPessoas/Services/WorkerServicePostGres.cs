using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Reflection.Metadata.Ecma335;
using DtoEntities = GestaoPessoas.Dtos;
using GestaoPessoas.DataSources;
using EfEntities = GestaoPessoas.Entities;

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

        public DtoEntities.Worker AddWorker(DtoEntities.Worker worker)
        {

            EfEntities.Worker new_worker = new EfEntities.Worker
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

        public IEnumerable<DtoEntities.Worker> GetAllWorkers()
        {
            List<DtoEntities.Worker> workers = (from w in dbContext.Workers
                                        orderby w.Name
                                        select new DtoEntities.Worker
                                        {
                                            Id = w.Id,
                                            Name = w.Name,
                                            JobTitle = w.JobTitle,
                                            Email = w.Email,
                                            BirthDate = w.BirthDate,
                                            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(w.TimeZone),
                                            PortalDasFinancasUserName = w.PortalDasFinancasUserName,
                                            PortalDasFinancasUserPassword = string.IsNullOrEmpty(w.PortalDasFinancasUserPassword)
                                                ? string.Empty
                                                : cryptoService.Decrypt(w.PortalDasFinancasUserPassword)!
                                        }).ToList();
            return workers;
        }

        public DtoEntities.Worker? GetWorkerByIdIfExists(int id)
        {
            DtoEntities.Worker? worker = (from w in dbContext.Workers
                                  where w.Id == id
                                  select new DtoEntities.Worker
                                  {
                                      Id = w.Id,
                                      Name = w.Name,
                                      JobTitle = w.JobTitle,
                                      Email = w.Email,
                                      BirthDate = w.BirthDate,
                                      TimeZone = TimeZoneInfo.FindSystemTimeZoneById(w.TimeZone),
                                      PortalDasFinancasUserName = w.PortalDasFinancasUserName,
                                      PortalDasFinancasUserPassword = string.IsNullOrEmpty(w.PortalDasFinancasUserPassword)
                                                ? string.Empty
                                                : cryptoService.Decrypt(w.PortalDasFinancasUserPassword)!

                                  }).SingleOrDefault();
            return worker;
        }

        public bool RemoveWorker(int id)
        {
            EfEntities.Worker? worker = (from w in dbContext.Workers
                                 where w.Id == id
                                 select w).SingleOrDefault();

            if (worker == null) return false;
            dbContext.Workers.Remove(worker);
            dbContext.SaveChanges();
            return true;

        }

        public DtoEntities.Worker? UpdateWorker(DtoEntities.Worker worker)
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

        public DtoEntities.Worker NonSensitiveWorker(DtoEntities.Worker worker)
        {
            worker.PortalDasFinancasUserPassword = String.Empty;
            return worker;
        }
    }
}

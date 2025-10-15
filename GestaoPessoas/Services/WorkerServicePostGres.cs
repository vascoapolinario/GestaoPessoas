using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Reflection.Metadata.Ecma335;
using dto = GestaoPessoas.Dtos;
using GestaoPessoas.DataSources;
using ef = GestaoPessoas.Entities;

namespace GestaoPessoas.Services
{
    public class WorkerServicePostGres : IWorkerService
    {
        private readonly GestaoPessoasDbContext dbContext;

        public WorkerServicePostGres(GestaoPessoasDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public dto.Worker AddWorker(dto.Worker worker)
        {

            ef.Worker new_worker = new ef.Worker
            {
                Name = worker.Name,
                JobTitle = worker.JobTitle,
                Email = worker.Email,
                BirthDate = worker.BirthDate,
                TimeZone = worker.TimeZone.Id
            };
            dbContext.Workers.Add(new_worker);
            dbContext.SaveChanges();
            return GetWorkerByIdIfExists(new_worker.Id)!;

        }

        public IEnumerable<dto.Worker> GetAllWorkers()
        {
            List<dto.Worker> workers = (from w in dbContext.Workers
                                        orderby w.Name
                                        select new dto.Worker
                                          {
                                              Id = w.Id,
                                              Name = w.Name,
                                              JobTitle = w.JobTitle,
                                              Email = w.Email,
                                              BirthDate = w.BirthDate,
                                              TimeZone = TimeZoneInfo.FindSystemTimeZoneById(w.TimeZone)
                                          }).ToList();
            return workers;
        }

        public dto.Worker? GetWorkerByIdIfExists(int id)
        {
            dto.Worker? worker = (from w in dbContext.Workers
                                    where w.Id == id
                                    select new dto.Worker
                                    {
                                        Id = w.Id,
                                        Name = w.Name,
                                        JobTitle = w.JobTitle,
                                        Email = w.Email,
                                        BirthDate = w.BirthDate,
                                        TimeZone = TimeZoneInfo.FindSystemTimeZoneById(w.TimeZone)
                                    }).SingleOrDefault();
            return worker;
        }

        public bool RemoveWorker(int id)
        {
            ef.Worker? worker = (from w in dbContext.Workers
                          where w.Id == id
                          select w).SingleOrDefault();

            if (worker == null) return false;
            dbContext.Workers.Remove(worker);
            dbContext.SaveChanges();
            return true;

        }

        public dto.Worker? UpdateWorker(dto.Worker worker)
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
            dbContext.SaveChanges();
            return GetWorkerByIdIfExists(worker.Id);
        }
    }
}

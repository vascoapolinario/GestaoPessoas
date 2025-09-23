using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Reflection.Metadata.Ecma335;
using GestaoPessoas.Dtos;

namespace GestaoPessoas.Services
{
    public class WorkerServicePostGres : IWorkerService
    {

        private readonly string? _connectionString;

        public WorkerServicePostGres(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private Worker DataToWorker(NpgsqlDataReader datareader) { 
            return new Worker
            {
                Id = datareader.GetInt32(datareader.GetOrdinal("id")),
                Name = datareader.GetString(datareader.GetOrdinal("name")),
                JobTitle = datareader.GetString(datareader.GetOrdinal("job_title")),
                Email = datareader.GetString(datareader.GetOrdinal("email")),
                BirthDate = DateOnly.FromDateTime(datareader.GetDateTime(datareader.GetOrdinal("birth_date")))
            };
        }

        public Worker AddWorker(Worker worker)
        {

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                using var cmd = new NpgsqlCommand($"INSERT INTO workers (name, job_title, email, birth_date) VALUES (@name, @job_title, @email, @birth_date) RETURNING id", conn);
                cmd.Parameters.AddWithValue("name", worker.Name);
                cmd.Parameters.AddWithValue("job_title", worker.JobTitle);
                cmd.Parameters.AddWithValue("email", worker.Email);
                cmd.Parameters.AddWithValue("birth_date", worker.BirthDate.ToDateTime(new TimeOnly(0, 0)));
                conn.Open();
                int? id = (int?)cmd.ExecuteScalar();
                if (id == null) throw new Exception("Failed to add the new worker");
                return GetWorkerByIdIfExists(id.Value);
            }
        }

        public IEnumerable<Worker> GetAllWorkers()
        {
            var workers = new List<Worker>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT id, name, job_title, email, birth_date FROM workers", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                workers.Add(DataToWorker(reader));
            }
            return workers;
        }

        public Worker? GetWorkerByIdIfExists(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT id, name, job_title, email, birth_date FROM workers WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return DataToWorker(reader);
            }
            else
            {
                return null;
            }
        }

        public bool RemoveWorker(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            using var transaction = conn.BeginTransaction();
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM workers WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            int affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows > 1)
            {
                transaction.Rollback();
                throw new Exception("Transaction cancelled, more than one worker found with the selected id.");
            }
            transaction.Commit();
            return (affectedRows == 1);

        }

        public Worker? UpdateWorker(Worker worker)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            using var cmd = new NpgsqlCommand(
                "UPDATE workers SET name = @name, job_title = @job_title, email = @email, birth_date = @birth_date WHERE id = @id", 
                conn, transaction);
            cmd.Parameters.AddWithValue("id", worker.Id);
            cmd.Parameters.AddWithValue("name", worker.Name);
            cmd.Parameters.AddWithValue("job_title", worker.JobTitle);
            cmd.Parameters.AddWithValue("email", worker.Email);
            cmd.Parameters.AddWithValue("birth_date", worker.BirthDate.ToDateTime(new TimeOnly(0, 0)));
            int affectedRows = cmd.ExecuteNonQuery();
            if (affectedRows > 1)
            {
                transaction.Rollback();
                throw new Exception("Transaction cancelled, more than one worker found with the selected id.");
            }
            transaction.Commit();
            return affectedRows == 1 ? GetWorkerByIdIfExists(worker.Id) : null;
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace GestaoPessoas.Services
{
    public class ProjectService
    {
        private readonly DataSources.GestaoPessoasDbContext _dbContext;
        public ProjectService(DataSources.GestaoPessoasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Dtos.Project> GetAllProjects()
        {
            var projects = from p in _dbContext.Projects
                           orderby p.Name
                           select new Dtos.Project
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Workers = p.Workers.Select(w => w.Name).ToList(),
                               WorkersCount = p.Workers.Count
                           };
            return projects.ToList();
        }

        public Dtos.Project? GetProjectByIdIfExists(int id)
        {
            var project = (from p in _dbContext.Projects
                           where p.Id == id
                           select new Dtos.Project
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Workers = p.Workers.Select(w => w.Name).ToList(),
                               WorkersCount = p.Workers.Count
                           }).FirstOrDefault();
            return project;
        }

        public Dtos.Project? AddProject(Dtos.Project project)
        {
            if (_dbContext.Projects.Any(p => p.Name == project.Name))
            {
                return null;
            }
            var newProject = new Entities.Project
            {
                Name = project.Name,
                Workers = new List<Entities.Worker>()
            };
            _dbContext.Projects.Add(newProject);
            _dbContext.SaveChanges();
            project.Id = newProject.Id;
            return GetProjectByIdIfExists(newProject.Id)!;
        }

        public bool RemoveProject(int id)
        {
            var projectToRemove = _dbContext.Projects.FirstOrDefault(p => p.Id == id);
            if (projectToRemove != null)
            {
                foreach (var worker in projectToRemove.Workers)
                {
                    worker.Projects.Remove(projectToRemove);
                }
                _dbContext.Set<Entities.Project>().Remove(projectToRemove);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Dtos.Project? UpdateProject(Dtos.Project project)
        {
            var existingProject = _dbContext.Projects.FirstOrDefault(p => p.Id == project.Id);
            if (project != null && _dbContext.Projects.Any(p => p.Name == project.Name && p.Id != project.Id))
            {
                return null;
            }
            if (existingProject != null)
            {
                existingProject.Name = project!.Name;
                _dbContext.SaveChanges();
                return GetProjectByIdIfExists(project.Id);
            }
            else
            {
                return null;
            }
        }

        public Dtos.Project? AddWorkerToProject(int projectId, int workerId)
        {
            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                var existingWorker = _dbContext.Set<Entities.Worker>().FirstOrDefault(w => w.Id == workerId);
                if (existingWorker != null)
                {
                    project.Workers.Add(existingWorker);
                    existingWorker.Projects.Add(project);
                    _dbContext.SaveChanges();
                }
                else
                {
                    return null;
                }
            }
            return GetProjectByIdIfExists(projectId)!;
        }

        public Dtos.Project? RemoveWorkerFromProject(int projectId, int workerId)
        {
            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                var workerToRemove = project.Workers.FirstOrDefault(w => w.Id == workerId);
                if (workerToRemove != null)
                {
                    project.Workers.Remove(workerToRemove);
                    workerToRemove.Projects.Remove(project);
                    _dbContext.SaveChanges();
                }
                else
                {
                    return null;
                }
            }
            return GetProjectByIdIfExists(projectId)!;
        }

        public bool IsWorkerInProject(int projectId, int workerId)
        {
            var project = _dbContext.Projects.Include(p => p.Workers).FirstOrDefault(p => p.Id == projectId);
            if (project != null)
            {
                return project.Workers.Any(w => w.Id == workerId);
            }
            return false;
        }
    }
}

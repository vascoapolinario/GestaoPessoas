using GestaoPessoas.DataSources;
using GestaoPessoas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dtos = GestaoPessoas.Dtos;

namespace GestaoPessoasTests.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class ProjectTests
    {
        private TestApplicationDomain? applicationDomain;
        private ProjectService service;

        [TestInitialize]
        public void Initialize()
        {
            applicationDomain = new TestApplicationDomain();
            string? _connectionString = applicationDomain.configuration.GetConnectionString("DefaultConnection");
            if (_connectionString == null) Assert.Inconclusive("Não foi possível obter a connection string de configuração.");
            applicationDomain.Services.AddDbContext<GestaoPessoasDbContext>(o => o.UseNpgsql(_connectionString));
            applicationDomain.Services.AddScoped<ProjectService>();
            service = applicationDomain.ServiceProvider.GetRequiredService<ProjectService>();

            string? BackupPath = applicationDomain.configuration.GetValue<string>("ProjectService:FilePath");
            if (BackupPath == null) throw new Exception("Não foi possível obter o caminho do ficheiro de configuração.");

            string sql = File.ReadAllText(BackupPath);

            using (IServiceScope serviceScope = applicationDomain.ServiceProvider.CreateScope())
            {
                GestaoPessoasDbContext databaseContext = serviceScope.ServiceProvider.GetRequiredService<GestaoPessoasDbContext>();
                databaseContext.Database.Migrate();
                databaseContext.Database.ExecuteSqlRaw(sql);
                databaseContext.SaveChanges();

            }
        }


        [TestMethod]
        public void ListProjectsTest()
        {
            var projects = service.GetAllProjects();
            Assert.AreEqual(5, projects.Count());
        }

        [TestMethod]
        public void GetProjectsTest()
        {
            var project = service.GetProjectByIdIfExists(2);
            Assert.IsNotNull(project);
            Assert.AreEqual("ProjectTest2", project.Name);
        }

        [TestMethod]
        public void GetProjectsNotFoundTest()
        {
            var project = service.GetProjectByIdIfExists(0);
            Assert.IsNull(project);
        }

        [TestMethod]
        public void AddProjectTest()
        {
            Dtos.Project newProject = new Dtos.Project
            {
                Name = "New Project"
            };

            Dtos.Project? addedProject = service.AddProject(newProject);
            Assert.IsNotNull(addedProject);
            Assert.AreEqual("New Project", addedProject.Name);
        }


        [TestMethod]
        public void RemoveProjectTest()
        {
            var success = service.RemoveProject(3);
            Assert.IsTrue(success);
            var project = service.GetProjectByIdIfExists(3);
            Assert.IsNull(project);
        }

        [TestMethod]
        public void RemoveNonExistingProjectTest()
        {
            var success = service.RemoveProject(0);
            Assert.IsFalse(success);
        }

        [TestMethod]
        public void UpdateProjectTest()
        {
            var existingProject = service.GetProjectByIdIfExists(4);
            Assert.IsNotNull(existingProject);
            existingProject.Name = "Updated Project Name";
            var updatedProject = service.UpdateProject(existingProject);
            Assert.IsNotNull(updatedProject);
            Assert.AreEqual("Updated Project Name", updatedProject.Name);
            var fetchedProject = service.GetProjectByIdIfExists(4);
            Assert.IsNotNull(fetchedProject);
            Assert.AreEqual("Updated Project Name", fetchedProject.Name);
        }

        [TestMethod]
        public void UpdateNonExistingProjectTest()
        {
            var nonExistingProject = new Dtos.Project
            {
                Id = 0,
                Name = "Non Existing Project"
            };
            var updatedProject = service.UpdateProject(nonExistingProject);
            Assert.IsNull(updatedProject);
        }

        [TestMethod]
        public void UpdateProjectDuplicateNameTest()
        {
            var existingProject = service.GetProjectByIdIfExists(5);
            Assert.IsNotNull(existingProject);
            existingProject.Name = "ProjectTest2";
            var updatedProject = service.UpdateProject(existingProject);
            Assert.IsNull(updatedProject);
        }

        [TestMethod]
        public void AddWorkerToProjectTest()
        {
            var project = service.GetProjectByIdIfExists(1);
            Assert.IsNotNull(project);
            Dtos.Project? ProjectWithAddedWorker = service.AddWorkerToProject(project.Id, 1);
            Assert.IsNotNull(ProjectWithAddedWorker);
            Assert.IsTrue(ProjectWithAddedWorker.Workers.Any(w => w.Equals("TestUser13")));
        }

        [TestMethod]
        public void RemoveWorkerFromProjectTest()
        {
            var project = service.GetProjectByIdIfExists(1);
            Assert.IsNotNull(project);

            Dtos.Project? ProjectWithAddedWorker = service.AddWorkerToProject(project.Id, 1);
            Assert.IsNotNull(ProjectWithAddedWorker);

            Dtos.Project? ProjectWithRemovedWorker = service.RemoveWorkerFromProject(ProjectWithAddedWorker.Id, 1);
            Assert.IsNotNull(ProjectWithRemovedWorker);
            Assert.IsFalse(ProjectWithRemovedWorker.Workers.Any(w => w.Equals("TestUser13")));
        }

        [TestCleanup]
        public void Cleanup()
        {
            applicationDomain?.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoPessoasTests
{
    internal class TestApplicationDomain
    {
        private ServiceProvider? serviceProvider = null;
        private readonly ServiceCollection services;

        internal ServiceCollection Services
        {
            get
            {
                if (serviceProvider != null) throw new Exception("Uma vez consultado o ServiceProvider já não é possível manipular a coleção de serviços. Se for preciso registar algum serviço faça antes de aceder ao ServiceProvider.");
                return services;
            }
        }

        public TestApplicationDomain() 
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<TestApplicationDomain>();
            var configuration = configurationBuilder.Build();
            services = new ServiceCollection();
            services.AddScoped(f => configuration);
        }
    }
}

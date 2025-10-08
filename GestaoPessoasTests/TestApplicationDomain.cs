using GestaoPessoas.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoPessoasTests
{
    public class TestApplicationDomain : IDisposable
    {
        private ServiceProvider? serviceProvider = null;
        private readonly ServiceCollection services;

        public ServiceCollection Services
        {
            get
            {
                if (serviceProvider != null) throw new Exception("Uma vez consultado o ServiceProvider já não é possível manipular a coleção de serviços. Se for preciso registar algum serviço faça antes de aceder ao ServiceProvider.");
                return services;
            }
        }

        public void Dispose()
        {
            if (serviceProvider != null)
            {
                serviceProvider.Dispose();
                serviceProvider = null;
            }
        }

        public ServiceProvider ServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                {
                    serviceProvider = services.BuildServiceProvider();
                }
                return serviceProvider;
            }
        }

        public TestApplicationDomain() 
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<TestApplicationDomain>();
            var configuration = configurationBuilder.Build();
            services = new ServiceCollection();
            services.AddScoped<IConfiguration>(f => configuration);
            Services.AddScoped<ICryptoService, AesCryptoService>();
            //services.AddScoped(f => configuration);
        }
    }
}

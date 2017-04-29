using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPIApplication;

namespace UnitTest
{
    //Classe responsavel por compartilhar recursos que seriam criados a cada teste rodado
    //Essa classe é rodada apenas uma vez
    //resumido: prepara o setup do database para os testes
    public class BaseTestFixture : IDisposable
    {
        public readonly TestServer Server;
        public readonly HttpClient Client;
        public readonly DataContext TestDataContext;
        public readonly IConfigurationRoot Configuration;

        public BaseTestFixture(){
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.{envName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var opts = new DbContextOptionsBuilder<DataContext>();
            opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            TestDataContext = new DataContext(opts.Options);
            SetupDatabase();

            //Inicia o server com a classe starup da nossa aplicação WebAPIAplication
            Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        private void SetupDatabase()
        {
            try
            {
                //Verifica se o banco de dados ja foi criado, se não ele cra um de acordo com a connection string 
                TestDataContext.Database.EnsureCreated();
                //Aplica as micgrations
                TestDataContext.Database.Migrate();
            }
            catch(Exception)
            {

            }
        }
        public void Dispose()
        {
            TestDataContext.Dispose();
            Client.Dispose();
            Server.Dispose();
         }

    }
}
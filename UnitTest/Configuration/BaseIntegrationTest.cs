
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using WebAPIApplication;
using Xunit;

namespace UnitTest
{
    //Classe base para nossos testes
    //Objetivo dessa classe é conter as propriedades da classe con configuração (BaseTestFixures)
    //para serem reutilizadas por todos os testes
    //resumido: serve pra receber tudo mastigado(configurado) e a cada teste ele zera o banco

    //Este decorator serve para garantir que tanto a minha classe de integração como a minha fixure 
    //sejam compartilhados por varias classes de teste
    [Collection("Base collection")]
    public abstract class BaseIntegrationTest    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;
        protected readonly DataContext TestDataContext;
        protected BaseTestFixture Fixure {get;}

        protected BaseIntegrationTest(BaseTestFixture fixure)
        {
            Fixure = fixure;

            TestDataContext = fixure.TestDataContext;
            Server = fixure.Server;
            Client = fixure.Client;

            ClearDb().Wait();
        }

        private async Task ClearDb()
        {
            var commands = new[]
            {
                "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",//desabilita as contraints
                "EXEC sp_MSForEachTable 'DELETE FROM ?'",
                "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'",//habilita constrains
            };

            await TestDataContext.Database.OpenConnectionAsync();

            foreach(var command in commands)
            {
                await TestDataContext.Database.ExecuteSqlCommandAsync(command);
            }

            TestDataContext.Database.CloseConnection();
        }
    }
}
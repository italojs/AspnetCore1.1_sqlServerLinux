using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAPIApplication;
using Xunit;

namespace UnitTest
{
    public class PessoasControllerIntegrationTest : BaseIntegrationTest
    {
        private const string urlRoute = "api/pessoas";
        public PessoasControllerIntegrationTest(BaseTestFixture fixure) : base(fixure)
        {
            
        }

        [Fact]
        public async Task DeveRetornarListaDePessoasVazia()
        {
            var response = await Client.GetAsync(urlRoute);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Pessoa>>(responseString);

            Assert.Equal(data.Count, 0);
        }
    }
}
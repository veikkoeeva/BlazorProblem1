using System.Net.Http;
using System.Threading.Tasks;
using TestProject1.Tests;
using Xunit;
using Xunit.Extensions.AssemblyFixture;

namespace TestProject1
{
    public class ApiTests: IAssemblyFixture<IntegrationTestFixture>
    {
        IntegrationTestFixture fixture;


        public ApiTests(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        public async Task Test1()
        {
            using(var client = new HttpClient())
            {
                string fetchUrl = $"{fixture.DefaultServerRootUrl}/WeatherForecast";
                HttpResponseMessage res = await client.GetAsync(fetchUrl).ConfigureAwait(false);

                //Why is this Not Found here when the server is started in IntegrationTestFixture?
                Assert.True(res.IsSuccessStatusCode);
            }

            Assert.True(fixture.IsTest());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class GettingOnCallDeveloper : IClassFixture<WebTestFixture>
    {
        private HttpClient Client;
        public GettingOnCallDeveloper(WebTestFixture fixture)
        {
            Client = fixture.CreateClient();
        }

        [Fact]
        public async Task CanGetTheOnCallDeveloper()
        {
            // what I am testing here is that I can do the GET /oncall
            // and I get a properly formatted response that includes the name
            // the the cache returns
            var response = await Client.GetAsync("/oncall");
            var content = await response.Content.ReadAsAsync<OnCallDeveloperResponse>();

            Assert.Equal("Shelly Johnson", content.developer);

        }
    }

    public class OnCallDeveloperResponse
    {
        public string developer { get; set; }
    }

}

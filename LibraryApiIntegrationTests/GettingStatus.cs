using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class GettingStatus : IClassFixture<WebTestFixture>
    {
        private HttpClient Client;

        public GettingStatus(WebTestFixture factory)
        {
            this.Client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetTheStatus()
        {
            var response = await Client.GetAsync("/status");
            Assert.True(response.IsSuccessStatusCode);

            var mediaType = response.Content.Headers.ContentType.MediaType;
            Assert.Equal("application/json", mediaType);

            var content = await response.Content.ReadAsAsync<StatusResponse>();

            Assert.Equal("Everything is Great!", content.message);
            Assert.Equal("Joe Schmidtly", content.checkedBy);
            Assert.Equal(new DateTime(1969, 4, 20, 23, 59, 00), content.whenLastChecked);

        }
    }

    /*
     * {
  "message": "Everything is Great!",
  "checkedBy": "Joe Schmidtly",
  "whenLastChecked": "2020-08-10T14:56:28.0356851-04:00"
}
     */


    public class StatusResponse
    {
        public string message { get; set; }
        public string checkedBy { get; set; }
        public DateTime whenLastChecked { get; set; }
    }
}

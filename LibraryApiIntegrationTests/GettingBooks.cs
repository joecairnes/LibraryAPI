using Microsoft.AspNetCore.Server.HttpSys;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class GettingBooks : IClassFixture<WebTestFixture>
    {
        private readonly HttpClient Client;
        public GettingBooks(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task HasTheRightData()
        {
            var response = await Client.GetAsync("/books");

            var content = await response.Content.ReadAsAsync<GetBooksResponse>();
            Assert.Equal(2, content.numberOfBooks);
        }
    }

    public class GetBooksResponse
    {
        public BookSummaryResponse[] books { get; set; }
        public string genreFilter { get; set; }
        public int numberOfBooks { get; set; }
    }

    public class BookSummaryResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int numberOfPages { get; set; }
    }

}

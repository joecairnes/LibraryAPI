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
            Assert.Equal(content.books.Length, content.numberOfBooks);
            Assert.Equal("Jaws", content.books[0].title);
            Assert.Null(content.genreFilter);
        }

        [Fact]
        public async Task CanFilterByGenre()
        {
            var response = await Client.GetAsync("/books?genre=Fiction");
            var content = await response.Content.ReadAsAsync<GetBooksResponse>();

            Assert.Equal(1, content.numberOfBooks);
            Assert.Equal("Fiction", content.genreFilter);
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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class AddingABook : IClassFixture<WebTestFixture>
    {
        private HttpClient Client;
        private HttpResponseMessage Response;

        public AddingABook(WebTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task HasCorrectStatusCode()
        {
            var bookToAdd = new PostBookRequest
            {
                title = "Walden",
                author = "Thoreau",
                genre = "Philosophy",
                numberOfPages = 223
            };
            var response = await Client.PostAsJsonAsync("/books", bookToAdd);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task HasLocationHeader()
        {
            var bookToAdd = new PostBookRequest
            {
                title = "Walden",
                author = "Thoreau",
                genre = "Philosophy",
                numberOfPages = 223
            };
            var response = await Client.PostAsJsonAsync("/books", bookToAdd);

            var content = await response.Content.ReadAsAsync<RooPostBookResponse>();
            var id = content.id;

            var header = response.Headers.Location.AbsoluteUri;

            Assert.Equal($"http://localhost/books/{id}", header);
        }

        [Fact]
        public async Task HasCorrectResponseBody()
        {
            var bookToAdd = new PostBookRequest
            {
                title = "Walden",
                author = "Thoreau",
                genre = "Philosophy",
                numberOfPages = 223
            };
            var response = await Client.PostAsJsonAsync("/books", bookToAdd);
            var content = await response.Content.ReadAsAsync<RooPostBookResponse>();

            Assert.Equal("Walden", content.title);
            // etc etc
            // it would be good to test something created by the service,
            // but that would only be the ID, and that is not consistent
        }


    }


    public class PostBookRequest
    {
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int numberOfPages { get; set; }
    }

    public class RooPostBookResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string genre { get; set; }
        public int numberOfPages { get; set; }
    }
}

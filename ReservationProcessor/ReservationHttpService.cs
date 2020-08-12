using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationProcessor
{
    public class ReservationHttpService
    {
        HttpClient Client;

        public ReservationHttpService(HttpClient client, IConfiguration config)
        {
            client.BaseAddress = new Uri(config.GetValue<string>("apiUrl"));
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "ReservationProcessor");
            Client = client;
        }

        public async Task<bool> MarkReservationApproved(Reservation reservation)
        {
            return await DoIt(reservation, "approved");
        }

        public async Task<bool> MarkReservationDenied(Reservation reservation)
        {
            return await DoIt(reservation, "denied");
        }

        private async Task<bool> DoIt(Reservation reservation, string status)
        {
            var reservationJson = JsonSerializer.Serialize(reservation);
            var content = new StringContent(reservationJson);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await Client.PostAsync($"/reservations/{status}", content);
            return response.IsSuccessStatusCode;
        }
    }
}

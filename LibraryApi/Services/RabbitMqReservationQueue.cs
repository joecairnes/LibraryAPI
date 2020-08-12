using LibraryApi.Models;
using RabbitMqUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class RabbitMqReservationQueue : ISendReservationToTheQueue
    {
        IRabbitManager Manager;

        public RabbitMqReservationQueue(IRabbitManager manager)
        {
            Manager = manager;
        }

        public void SendReservation(ReservationItem response)
        {
            Manager.Publish(response, "", "direct", "reservations");
        }
    }
}

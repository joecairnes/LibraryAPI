using LibraryApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class GetReservationsResponse
    {
        public List<ReservationItem> Reservations { get; set; }
    }

    public class ReservationItem
    {
        public int Id { get; set; }
        public string For { get; set; }
        public string Books { get; set; }
        public ReservationStatus Status { get; set; }

    }
}

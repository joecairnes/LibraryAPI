using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface ISendReservationToTheQueue
    {
        void SendReservation(ReservationItem response);
    }
}
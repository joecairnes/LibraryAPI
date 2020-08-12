using LibraryApi.Domain;
using LibraryApi.Filters;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController : ControllerBase
    {
        LibraryDataContext Context;
        ISendReservationToTheQueue Queue;

        public ReservationsController(LibraryDataContext context, ISendReservationToTheQueue queue)
        {
            Context = context;
            Queue = queue;
        }

        // GET /reservations - returns all the reservations (pending, approved, denied)
        [HttpGet("/reservations")]
        public async Task<ActionResult<GetReservationsResponse>> GetAllReservations()
        {
            var reservations = await Context.Reservations
                .Select(r => new ReservationItem
                {
                    Id = r.Id,
                    For = r.For,
                    Books = r.Books,
                    Status = r.Status
                }).ToListAsync();

            var response = new GetReservationsResponse { Reservations = reservations };
            return Ok(response);
        }
        // POST /reservations - add a reservation
        [HttpPost("/reservations")]
        [ValidateModel]
        public async Task<ActionResult<ReservationItem>> AddReservation([FromBody] PostReservationRequest item)
        {
            var reservation = new Reservation
            {
                For = item.For,
                Books = item.Books,
                Status = ReservationStatus.Pending
            };
            Context.Reservations.Add(reservation);
            await Context.SaveChangesAsync();

            var response = new ReservationItem
            {
                Id = reservation.Id,
                For = reservation.For,
                Books = reservation.Books,
                Status = reservation.Status
            };

            // Write it to the Queue for the background worker  to process!
            // WTCYWYH
           
            Queue.SendReservation(response);
            return Ok(response);
        }

        // GET /reservations/approved - returns all the approved reservations
        [HttpGet("/reservations/approved")]
        public async Task<ActionResult<GetReservationsResponse>> GetApprovedReservations()
        {
            var reservations = await Context.Reservations
                .Where(r => r.Status == ReservationStatus.Approved)
                .Select(r => new ReservationItem
                {
                    Id = r.Id,
                    For = r.For,
                    Books = r.Books,
                    Status = r.Status
                }).ToListAsync();

            var response = new GetReservationsResponse { Reservations = reservations };
            return Ok(response);
        }
        // POST /reservations/approved (auth) - send a reservation here to mark it as approved
        [HttpPost("/reservations/approved")]
        public async Task<ActionResult<ReservationItem>> Approve([FromBody] ReservationItem reservation)
        {

            var stored = await Context.Reservations
                .SingleOrDefaultAsync(r => r.Id == reservation.Id);
            if (stored == null)
            {
                return BadRequest("No reservation with that Id.");
            }
            else
            {
                stored.Status = ReservationStatus.Approved;
                await Context.SaveChangesAsync();
                return NoContent(); // Fine.
            }
        }

        // GET /reservations/denied - return the denied reservations
        [HttpGet("/reservations/denied")]
        public async Task<ActionResult<GetReservationsResponse>> GetDeniedReservations()
        {
            var reservations = await Context.Reservations
                .Where(r => r.Status == ReservationStatus.Denied)
                .Select(r => new ReservationItem
                {
                    Id = r.Id,
                    For = r.For,
                    Books = r.Books,
                    Status = r.Status
                }).ToListAsync();

            var response = new GetReservationsResponse { Reservations = reservations };
            return Ok(response);
        }
        // POST /reservations/denied (auth) - send a reservation here to mark it as denied
        [HttpPost("/reservations/denied")]
        public async Task<ActionResult<ReservationItem>> Deny([FromBody] ReservationItem reservation)
        {

            var stored = await Context.Reservations
                .SingleOrDefaultAsync(r => r.Id == reservation.Id);
            if (stored == null)
            {
                return BadRequest("No reservation with that Id.");
            }
            else
            {
                stored.Status = ReservationStatus.Denied;
                await Context.SaveChangesAsync();
                return NoContent(); // Fine.
            }
        }
        // GET /reservations/pending - return all the pending reservations
        [HttpGet("/reservations/pending")]
        public async Task<ActionResult<GetReservationsResponse>> GetPendingReservations()
        {

            var reservations = await Context.Reservations
                .Where(r => r.Status == ReservationStatus.Pending)
                .Select(r => new ReservationItem
                {
                    Id = r.Id,
                    For = r.For,
                    Books = r.Books,
                    Status = r.Status
                }).ToListAsync();

            var response = new GetReservationsResponse { Reservations = reservations };
            return Ok(response);

        }
    }
}


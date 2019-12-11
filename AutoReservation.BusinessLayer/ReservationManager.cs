using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public async Task<List<Reservation>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Reservationen
                .Include(r => r.Auto) //Eager Loading
                .Include(r => r.Kunde) //Eager Loading
                .ToListAsync();
        }

        public async Task<Reservation> Get(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return await context.Reservationen
                    .Where(r => r.ReservationsNr == id)
                    .Include(r => r.Auto) //Eager Loading
                    .Include(r => r.Kunde) //Eager Loading
                    .FirstAsync();
            }
        }

        public async Task<Reservation> Insert(Reservation reservation)
        {
            if (CheckDateRange(reservation) && await IsCarAvailable(reservation))
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    context.Entry(reservation).State = EntityState.Added;
                    await context.SaveChangesAsync();
                    return reservation;
                }
            }

            return null;
        }

        public async Task Update(Reservation reservation)
        {
            if (CheckDateRange(reservation) && await IsCarAvailable(reservation))
            {
                using (AutoReservationContext context = new AutoReservationContext())
                {
                    try
                    {
                        context.Entry(reservation).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException exception)
                    {
                        throw CreateOptimisticConcurrencyException(context, reservation);
                    }
                }
            }
        }

        public async Task Delete(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(reservation).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public bool CheckDateRange(Reservation reservation)
        {
            TimeSpan timeSpan = reservation.Bis.Subtract(reservation.Von);
            if (timeSpan.Hours < 24 && timeSpan.Days < 1)
            {
                throw new InvalidDateRangeException("From-To must be at least 24 hours apart.");
            }

            return true;
        }

        public async Task<bool> IsCarAvailable(Reservation reservation)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                var query = from existingReservation in context.Reservationen
                    where existingReservation.AutoId == reservation.AutoId &&
                          (existingReservation.Von < reservation.Von && reservation.Von < existingReservation.Bis) || /* Von zw. bestehendem Von - Bis  */ 
                          (existingReservation.Von < reservation.Bis && reservation.Bis < existingReservation.Bis) || /* Bis zw. bestehendem Von - Bis  */ 
                          (existingReservation.Von == reservation.Von) ||
                          (existingReservation.Bis == reservation.Bis) 
                    select existingReservation;
                Reservation result = await query.FirstOrDefaultAsync();
                if (result != null)
                {
                    throw new AutoUnavailableException("Car is not available.");
                }
            }
            return true;
        }
    }
}
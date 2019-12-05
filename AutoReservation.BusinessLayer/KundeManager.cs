using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class KundeManager
        : ManagerBase
    {
        public async Task<List<Kunde>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Kunden.ToListAsync();
        }

        public async Task<Kunde> Get(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return await context.Kunden.FindAsync(id);
            }
        }

        public async Task<Kunde> Insert(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(kunde).State = EntityState.Added;
                await context.SaveChangesAsync();
                return kunde;
            }
        }

        public async Task Update(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Entry(kunde).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    throw CreateOptimisticConcurrencyException(context, kunde);
                }
            }
        }

        public async Task Delete(Kunde kunde)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(kunde).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}
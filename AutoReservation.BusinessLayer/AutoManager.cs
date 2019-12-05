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
    public class AutoManager
        : ManagerBase
    {
        // Example
        public async Task<List<Auto>> GetAll()
        {
            using AutoReservationContext context = new AutoReservationContext();
            return await context.Autos.ToListAsync();
        }

        public async Task<Auto> Get(int id)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                return await context.Autos.FindAsync(id);
            }
        }

        public async Task<Auto> Insert(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(auto).State = EntityState.Added;
                await context.SaveChangesAsync();
                return auto;
            }
        }

        public async Task Update(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                try
                {
                    context.Entry(auto).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException exception)
                {
                    throw CreateOptimisticConcurrencyException(context, auto);
                }
            }
        }

        public async Task Delete(Auto auto)
        {
            using (AutoReservationContext context = new AutoReservationContext())
            {
                context.Entry(auto).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}
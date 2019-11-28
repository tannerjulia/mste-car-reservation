using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationUpdateTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            // arrange
            Reservation reservation = await _target.Get(1);
            reservation.Von = new DateTime(2020, 10, 03);
            reservation.Bis = new DateTime(2020, 10, 18);

            // act
            await _target.Update(reservation);

            // assert
            reservation = await _target.Get(1);
            Assert.Equal(1, reservation.ReservationsNr);
            Assert.Equal(new DateTime(2020, 10, 03), reservation.Von);
            Assert.Equal(new DateTime(2020, 10, 18), reservation.Bis);
        }

        [Fact]
        public async Task UpdateReservationExceptionTest()
        {
            // arrange
            Reservation reservation1 = await _target.Get(1);
            reservation1.Von = new DateTime(2020, 10, 21);
            reservation1.Bis = new DateTime(2020, 10, 23);

            Reservation reservation2 = await _target.Get(1);
            reservation2.Von = new DateTime(2020, 10, 24);
            reservation2.Bis = new DateTime(2020, 10, 26);
            
            // act
            await _target.Update(reservation1);
            await Assert.ThrowsAsync<OptimisticConcurrencyException<Reservation>>(() => _target.Update(reservation2));

            // assert
            reservation1 = await _target.Get(1);
            Assert.Equal(1, reservation1.ReservationsNr);
            Assert.Equal(new DateTime(2020,10,21), reservation1.Von); 
            Assert.Equal(new DateTime(2020, 10, 23), reservation1.Bis);
        }
    }
}

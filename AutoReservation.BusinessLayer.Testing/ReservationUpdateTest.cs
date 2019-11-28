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
            reservation.Von = DateTime.Parse("03/10/2020");
            reservation.Bis = DateTime.Parse("18/10/2020");

            // act
            await _target.Update(reservation);

            // assert
            reservation = await _target.Get(1);
            Assert.Equal(1, reservation.ReservationsNr);
            Assert.Equal(DateTime.Parse("03/10/2020"), reservation.Von);
            Assert.Equal(DateTime.Parse("18/10/2020"), reservation.Bis);
        }

        [Fact]
        public async Task UpdateReservationExceptionTest()
        {
            // arrange
            Reservation reservation1 = await _target.Get(1);
            reservation1.Von = DateTime.Parse("21/10/2020");
            reservation1.Bis = DateTime.Parse("23/10/2020");

            Reservation reservation2 = await _target.Get(1);
            reservation2.Von = DateTime.Parse("24/10/2020");
            reservation2.Bis = DateTime.Parse("26/10/2020");
            
            // act
            await _target.Update(reservation1);
            await Assert.ThrowsAsync<OptimisticConcurrencyException<Reservation>>(() => _target.Update(reservation2));

            // assert
            reservation1 = await _target.Get(1);
            Assert.Equal(1, reservation1.ReservationsNr);
            Assert.Equal(DateTime.Parse("21/10/2020"), reservation1.Von);
            Assert.Equal(DateTime.Parse("23/10/2020"), reservation1.Bis);
        }
    }
}

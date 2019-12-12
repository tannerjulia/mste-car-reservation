using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task ScenarioOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //               | ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020,01, 20),
                Bis = new DateTime(2020, 01,21)
            };
            
            // act
            bool result = await _target.IsCarAvailable(reservation);
            
            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 21),
                Bis = new DateTime(2020, 01, 22)
            };
            
            // act
            bool result = await _target.IsCarAvailable(reservation);
            
            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2-- - |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 09),
                Bis = new DateTime(2020, 01, 10)
            };
            
            // act
            bool result = await _target.IsCarAvailable(reservation);
            
            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 08, 00, 00,00),
                Bis = new DateTime(2020, 01, 09, 00,00,00)
            };
            
            // act
            bool result = await _target.IsCarAvailable(reservation);
            
            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ScenarioNotOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //    | ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 15, 00, 00, 00),
                Bis = new DateTime(2020, 01, 25, 00, 00, 00)
            };
            
            // act - assert
            await Assert.ThrowsAsync<AutoUnavailableException>(() => _target.IsCarAvailable(reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay02Test()
        {
            // arrange
            //    | ---Date 1--- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 05, 00, 00, 00),
                Bis = new DateTime(2020, 01, 15, 00,00,00)
            };
            
            // act - assert
            await Assert.ThrowsAsync<AutoUnavailableException>(() => _target.IsCarAvailable(reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay03Test()
        {
            // arrange
            //| ---Date 1--- |
            //| --------Date 2-------- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 10),
                Bis = new DateTime(2020, 01, 25)
            };
            
            // act - assert
            await Assert.ThrowsAsync<AutoUnavailableException>(() => _target.IsCarAvailable(reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay04Test()
        {
            // arrange
            //| --------Date 1-------- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 10),
                Bis = new DateTime(2020, 01, 15)
            };
            
            // act - assert
            await Assert.ThrowsAsync<AutoUnavailableException>(() => _target.IsCarAvailable(reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay05Test()
        {
            // arrange
            //| ---Date 1--- |
            //| ---Date 2--- |
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 01, 10),
                Bis = new DateTime(2020, 01, 20)
            };
            
            // act - assert
            await Assert.ThrowsAsync<AutoUnavailableException>(() => _target.IsCarAvailable(reservation));
        }
    }
}

﻿using System;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationDateRangeTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public void ScenarioOkay01TestAsync()
        {
            // arrange
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020,10,04), 
                Bis = new DateTime(2020, 11, 19),
            };

            // act
            bool result = _target.CheckDateRange(reservation);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            // arrange
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 10, 04, 12, 00, 00),
                Bis = new DateTime(2020, 10, 05, 12, 00, 00)
            };

            // act
            bool result = _target.CheckDateRange(reservation);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            // arrange
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020,10,04, 12, 00, 00), 
                Bis = new DateTime(2020, 10, 05, 11, 59, 59)
            };

            // act - assert
            Assert.Throws<InvalidDateRangeException>(() => _target.CheckDateRange(reservation));
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            // arrange
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 10, 04, 12, 00, 00),
                Bis = new DateTime(2020, 10, 04, 11, 59, 59)
            };

            // act - assert
            Assert.Throws<InvalidDateRangeException>(() =>  _target.CheckDateRange(reservation));
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            // arrange
            Reservation reservation = new Reservation
            {
                KundeId = 1,
                AutoId = 1,
                Von = new DateTime(2020, 10, 05),
                Bis = new DateTime(2020, 10, 04)
            };

            // act - assert
            Assert.Throws<InvalidDateRangeException>(() => _target.CheckDateRange(reservation));
        }
    }
}
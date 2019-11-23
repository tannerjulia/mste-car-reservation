using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly AutoManager _target;

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            // arrange
            Auto auto = await _target.Get(2);
            auto.Marke = "VW Polo";
            auto.Tagestarif = 135;

            // act
            await _target.Update(auto);

            // assert
            auto = await _target.Get(2);
            Assert.True(auto is MittelklasseAuto);
            Assert.Equal(2, auto.Id);
            Assert.Equal("VW Polo", auto.Marke);
            Assert.Equal(135, auto.Tagestarif);
        }

        [Fact]
        public async Task UpdateAutoExceptionTest()
        {
            // arrange
            Auto auto1 = await _target.Get(2);
            auto1.Marke = "VW Polo";
            auto1.Tagestarif = 135;

            Auto auto2 = await _target.Get(2);
            auto2.Marke = "VW Tiguan";
            auto2.Tagestarif = 150;

            //act
            await _target.Update(auto1);
            await Assert.ThrowsAsync<OptimisticConcurrencyException<Auto>>(() => _target.Update(auto2));

            //assert
            Auto auto = await _target.Get(2);
            Assert.True(auto is MittelklasseAuto);
            Assert.Equal(2, auto.Id);
            Assert.Equal("VW Polo", auto.Marke);
            Assert.Equal(135, auto.Tagestarif);
        }
    }
}
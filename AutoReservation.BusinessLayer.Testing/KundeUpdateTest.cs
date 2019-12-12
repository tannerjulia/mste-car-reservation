using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class KundeUpdateTest
        : TestBase
    {
        private readonly KundeManager _target;

        public KundeUpdateTest()
        {
            _target = new KundeManager();
        }

        [Fact]
        public async Task UpdateKundeTest()
        {
            // arrange
            Kunde kunde = await _target.Get(1);
            kunde.Nachname = "Tanner";
            kunde.Vorname = "Julia";
            kunde.Geburtsdatum = new DateTime(1996, 10, 04);

            // act
            await _target.Update(kunde);

            // assert
            kunde = await _target.Get(1);
            Assert.Equal(1, kunde.Id);
            Assert.Equal("Tanner", kunde.Nachname);
            Assert.Equal("Julia", kunde.Vorname);
            Assert.Equal(new DateTime(1996, 10, 04), kunde.Geburtsdatum);
        }

        [Fact]
        public async Task UpdateKundeExceptionTest()
        {
            // arrange
            Kunde kunde1 = await _target.Get(1);
            kunde1.Nachname = "Tanner";
            kunde1.Vorname = "Julia";
            kunde1.Geburtsdatum = new DateTime(1996, 10, 04);

            Kunde kunde2 = await _target.Get(1);
            kunde2.Nachname = "Gabriel";
            kunde2.Vorname = "Dominic";
            kunde2.Geburtsdatum = new DateTime(1994, 05, 01);

            // act
            await _target.Update(kunde1);
            await Assert.ThrowsAsync<OptimisticConcurrencyException<Kunde>>(() => _target.Update(kunde2));

            // assert
            kunde1 = await _target.Get(1);
            Assert.Equal(1, kunde1.Id);
            Assert.Equal("Tanner", kunde1.Nachname);
            Assert.Equal("Julia", kunde1.Vorname);
            Assert.Equal(new DateTime(1996, 10, 04), kunde1.Geburtsdatum);
        }
    }
}
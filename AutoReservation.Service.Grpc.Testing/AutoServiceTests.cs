﻿using System;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using AutoReservation.Service.Grpc.Testing.Common;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing
{
    public class AutoServiceTests
        : ServiceTestBase
    {
        private readonly AutoService.AutoServiceClient _target;

        public AutoServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new AutoService.AutoServiceClient(Channel);
        }

        [Fact]
        public async Task GetAutosTest()
        {
            // arrange
            AutosDto autos = _target.GetAll(new Empty());
            RepeatedField<AutoDto> autosDtos = autos.Cars;

            // act

            // assert
            Assert.Equal(4, autosDtos.Count);
            CompareAutoDtos(autosDtos[0], 1, "Fiat Punto", 50, AutoKlasse.Standard, 0);
            CompareAutoDtos(autosDtos[1], 2, "VW Golf", 120, AutoKlasse.Mittelklasse, 0);
            CompareAutoDtos(autosDtos[2], 3, "Audi S6", 180, AutoKlasse.Luxusklasse, 50);
            CompareAutoDtos(autosDtos[3], 4, "Fiat 500", 75, AutoKlasse.Standard, 0);
        }

        private void CompareAutoDtos(AutoDto auto, int id, string marke, int tagestarif, AutoKlasse autoklasse,
            int? basistarif)
        {
            Assert.Equal(id, auto.Id);
            Assert.Equal(marke, auto.Marke);
            Assert.Equal(tagestarif, auto.Tagestarif);
            Assert.Equal(autoklasse, auto.AutoKlasse);
            Assert.Equal(basistarif, auto.Basistarif);
        }

        [Fact]
        public async Task GetAutoByIdTest()
        {
            // arrange
            AutoDto auto1 = _target.Get(new AutoRequest {Id = 1});
            AutoDto auto2 = _target.Get(new AutoRequest {Id = 2});
            AutoDto auto3 = _target.Get(new AutoRequest {Id = 3});
            AutoDto auto4 = _target.Get(new AutoRequest {Id = 4});

            // act

            // assert
            CompareAutoDtos(auto1, 1, "Fiat Punto", 50, AutoKlasse.Standard, 0);
            CompareAutoDtos(auto2, 2, "VW Golf", 120, AutoKlasse.Mittelklasse, 0);
            CompareAutoDtos(auto3, 3, "Audi S6", 180, AutoKlasse.Luxusklasse, 50);
            CompareAutoDtos(auto4, 4, "Fiat 500", 75, AutoKlasse.Standard, 0);
        }

        [Fact]
        public async Task GetAutoByIdWithIllegalIdTest()
        {
            // arrange
            RpcException exception = Assert.Throws<RpcException>(() => _target.Get(new AutoRequest {Id = 5}));

            // act

            // assert
            Assert.Equal(StatusCode.OutOfRange, exception.StatusCode);
            Assert.Equal("Status(StatusCode=OutOfRange, Detail=\"Id couldn't be found.\")", exception.Message);
        }

        [Fact]
        public async Task InsertAutoTest()
        {
            // arrange
            string marke = "Ford Ranger";
            int tagestarif = 160;
            AutoKlasse autoklasse = AutoKlasse.Luxusklasse;
            int basistarif = 60;

            AutoDto auto = new AutoDto();
            auto.Marke = marke;
            auto.Tagestarif = tagestarif;
            auto.AutoKlasse = autoklasse;
            auto.Basistarif = basistarif;

            // act
            AutoDto autoDto = _target.Insert(auto);
            AutoDto auto1 = _target.Get(new AutoRequest {Id = autoDto.Id});

            // assert
            CompareAutoDtos(auto1, autoDto.Id, marke, tagestarif, autoklasse, basistarif);
        }

        [Fact]
        public async Task DeleteAutoTest()
        {
            // arrange
            int autoDeleteId = 1;
            AutoDto auto1 = _target.Get(new AutoRequest {Id = autoDeleteId});

            // act
            _target.Delete(auto1);

            // assert
            RpcException exception =
                Assert.Throws<RpcException>(() => _target.Get(new AutoRequest {Id = autoDeleteId}));
            Assert.Equal(StatusCode.OutOfRange, exception.StatusCode);
            Assert.Equal("Status(StatusCode=OutOfRange, Detail=\"Id couldn't be found.\")", exception.Message);
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            // arrange
            int autoUpdateId = 2;
            string autoUpdateMarke = "VW Gold e";

            AutoDto autoUpdate = _target.Get(new AutoRequest {Id = autoUpdateId});
            autoUpdate.Marke = autoUpdateMarke;

            // act
            _target.Update(autoUpdate);
            AutoDto autoUpdated = _target.Get(new AutoRequest {Id = autoUpdateId});

            // assert
            CompareAutoDtos(autoUpdated, autoUpdateId, autoUpdateMarke, 120, AutoKlasse.Mittelklasse, 0);
        }

        [Fact]
        public async Task UpdateAutoWithOptimisticConcurrencyTest()
        {
            // arrange
            AutoDto auto1 = _target.Get(new AutoRequest {Id = 2});
            auto1.Marke = "VW Polo";
            auto1.Tagestarif = 135;

            AutoDto auto2 = _target.Get(new AutoRequest {Id = 2});
            auto2.Marke = "VW Tiguan";
            auto2.Tagestarif = 150;

            //act
            _target.Update(auto1);
            RpcException exception = Assert.Throws<RpcException>(() => _target.Update(auto2));
            Assert.Equal(StatusCode.Aborted, exception.StatusCode);
            Assert.Equal("Status(StatusCode=Aborted, Detail=\"Conccurency Exception.\")", exception.Message);

            //assert
            AutoDto auto = _target.Get(new AutoRequest {Id = 2});
            CompareAutoDtos(auto, 2, "VW Polo", 135, AutoKlasse.Mittelklasse, 0);
        }
    }
}
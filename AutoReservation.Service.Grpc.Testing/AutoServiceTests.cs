using System;
using System.Threading.Tasks;
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
            Assert.Equal(4,autosDtos.Count);
            AssertAutoDto(autosDtos[0], 1, "Fiat Punto", 50, AutoKlasse.Standard, null);
            AssertAutoDto(autosDtos[1], 2, "VW Golf", 120, AutoKlasse.Mittelklasse, null);
            AssertAutoDto(autosDtos[2], 3, "Audi S6", 180, AutoKlasse.Luxusklasse, 50);
            AssertAutoDto(autosDtos[3], 4, "Fiat 500", 75, AutoKlasse.Standard, null);

        }

        private void AssertAutoDto(AutoDto auto, int id, string marke, int tagestarif, AutoKlasse autoklasse, int? basistarif) {
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
            AutoDto auto1 = _target.Get(new AutoRequest { Id = 1 } );
            AutoDto auto2 = _target.Get(new AutoRequest { Id = 2 } );
            AutoDto auto3 = _target.Get(new AutoRequest { Id = 3 } );
            AutoDto auto4 = _target.Get(new AutoRequest { Id = 4 } );
            
            // act
            
            // assert
            AssertAutoDto(auto1, 1, "Fiat Punto", 50, AutoKlasse.Standard, null);
            AssertAutoDto(auto2, 2, "VW Golf", 120, AutoKlasse.Mittelklasse, null);
            AssertAutoDto(auto3, 3, "Audi S6", 180, AutoKlasse.Luxusklasse, 50);
            AssertAutoDto(auto4, 4, "Fiat 500", 75, AutoKlasse.Standard, null);
        }

        [Fact]
        public async Task GetAutoByIdWithIllegalIdTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task InsertAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task DeleteAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateAutoTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }

        [Fact]
        public async Task UpdateAutoWithOptimisticConcurrencyTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}
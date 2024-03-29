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
    public class KundeServiceTests
        : ServiceTestBase
    {
        private readonly KundeService.KundeServiceClient _target;

        public KundeServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetKundenTest()
        {
            // arrange
            KundenDto kunden = _target.GetAll(new Empty());
            RepeatedField<KundeDto> kundenDtos = kunden.Customers;
            
            // act
            // assert
            Assert.Equal(4,kundenDtos.Count);
            CompareKundenDtos(kundenDtos[0], 1,"Nass", "Anna", Timestamp.FromDateTime(new DateTime(1981, 05, 05 , 0,0,0, DateTimeKind.Utc )));
            CompareKundenDtos(kundenDtos[1], 2, "Beil", "Timo", Timestamp.FromDateTime(new DateTime(1980, 09, 09, 0,0,0, DateTimeKind.Utc)));
            CompareKundenDtos(kundenDtos[2], 3, "Pfahl", "Martha", Timestamp.FromDateTime(new DateTime(1990, 07, 03, 0,0,0, DateTimeKind.Utc)));
            CompareKundenDtos(kundenDtos[3], 4, "Zufall", "Rainer", Timestamp.FromDateTime(new DateTime(1954, 11, 11, 0,0,0, DateTimeKind.Utc)));
        }
        
        private void CompareKundenDtos(KundeDto kunde, int id, string nachname, string vorname, Timestamp geburtsdatum) {
            Assert.Equal(id, kunde.Id);
            Assert.Equal(nachname, kunde.Nachname);
            Assert.Equal(vorname, kunde.Vorname);
            Assert.Equal(geburtsdatum, kunde.Geburtsdatum);
        }

        [Fact]
        public async Task GetKundeByIdTest()
        {
            // arrange
            KundeDto kunde1 = _target.Get(new KundeRequest { Id = 1 } );
            KundeDto kunde2 = _target.Get(new KundeRequest { Id = 2 } );
            KundeDto kunde3 = _target.Get(new KundeRequest { Id = 3 } );
            KundeDto kunde4 = _target.Get(new KundeRequest { Id = 4 } );
            
            // act
            
            // assert
            CompareKundenDtos(kunde1, 1, "Nass", "Anna", Timestamp.FromDateTime(new DateTime(1981, 05, 05, 0,0,0, DateTimeKind.Utc)));
            CompareKundenDtos(kunde2, 2, "Beil", "Timo", Timestamp.FromDateTime(new DateTime(1980, 09, 09, 0,0,0, DateTimeKind.Utc)));
            CompareKundenDtos(kunde3, 3, "Pfahl", "Martha", Timestamp.FromDateTime(new DateTime(1990, 07, 03, 0,0,0, DateTimeKind.Utc)));
            CompareKundenDtos(kunde4, 4, "Zufall", "Rainer", Timestamp.FromDateTime(new DateTime(1954, 11, 11, 0,0,0, DateTimeKind.Utc)));
        }

        [Fact]
        public async Task GetKundeByIdWithIllegalIdTest()
        {
            // arrange
            RpcException exception = Assert.Throws<RpcException>(() => _target.Get(new KundeRequest { Id = 5 } ));

            // act

            // assert
            Assert.Equal(StatusCode.OutOfRange, exception.StatusCode);
            Assert.Equal("Status(StatusCode=OutOfRange, Detail=\"Id couldn't be found.\")", exception.Message);
            
        }

        [Fact]
        public async Task InsertKundeTest()
        {
            // arrange
            string nachname = "Gabriel";
            string vorname = "Dominic";
            Timestamp geburtsdatum = Timestamp.FromDateTime(new DateTime(1981, 05, 05, 0,0,0, DateTimeKind.Utc));
            
            KundeDto kunde = new KundeDto();
            kunde.Nachname = nachname;
            kunde.Vorname = vorname;
            kunde.Geburtsdatum = geburtsdatum;
            
            // act
            KundeDto kundeDto = _target.Insert(kunde);
            KundeDto kunde1 = _target.Get(new KundeRequest {Id = kundeDto.Id});

            // assert
            CompareKundenDtos(kunde1, kundeDto.Id, nachname, vorname, geburtsdatum);
        }

        [Fact]
        public async Task DeleteKundeTest()
        {
            
            // arrange
            int kundeDeleteId = 1;
            KundeDto kunde1 = _target.Get(new KundeRequest {Id = kundeDeleteId});

            // act
            _target.Delete(kunde1);

            // assert
            RpcException exception =
                Assert.Throws<RpcException>(() => _target.Get(new KundeRequest() {Id = kundeDeleteId}));
            Assert.Equal(StatusCode.OutOfRange, exception.StatusCode);
            Assert.Equal("Status(StatusCode=OutOfRange, Detail=\"Id couldn't be found.\")", exception.Message);
        }

        [Fact]
        public async Task UpdateKundeTest()
        {
            // arrange
            int kundeUpdateId = 2;
            string kundeUpdateNachname = "Muster";
            string kundeUpdateVorname = "Hans";
            Timestamp kundeUpdateGeburtsdatum = Timestamp.FromDateTime(new DateTime(2000, 01, 01, 0,0,0, DateTimeKind.Utc));

            KundeDto kundeUpdate = _target.Get(new KundeRequest { Id = kundeUpdateId } );
            kundeUpdate.Id = kundeUpdateId;
            kundeUpdate.Nachname = kundeUpdateNachname;
            kundeUpdate.Vorname = kundeUpdateVorname;
            kundeUpdate.Geburtsdatum = kundeUpdateGeburtsdatum;
            
            // act
            _target.Update(kundeUpdate);
            KundeDto kundeUpdated = _target.Get(new KundeRequest { Id = kundeUpdateId } );

            // assert
            CompareKundenDtos(kundeUpdated, kundeUpdateId, kundeUpdateNachname, kundeUpdateVorname, kundeUpdateGeburtsdatum);
        }

        [Fact]
        public async Task UpdateKundeWithOptimisticConcurrencyTest()
        {
            // arrange
            KundeDto kunde1 = _target.Get(new KundeRequest { Id = 2 });
            kunde1.Nachname = "Bar";
            kunde1.Vorname = "Foo";
            kunde1.Geburtsdatum = Timestamp.FromDateTime(new DateTime(2000, 01, 01, 0,0,0, DateTimeKind.Utc));


            KundeDto kunde2 = _target.Get(new KundeRequest {Id = 2});
            kunde2.Nachname = "Test";
            kunde2.Vorname = "Random";
            kunde2.Geburtsdatum = Timestamp.FromDateTime(new DateTime(2001, 01, 01, 0,0,0, DateTimeKind.Utc));

            //act
            _target.Update(kunde1);
            
            RpcException exception = Assert.Throws<RpcException>(() => _target.Update(kunde2));
            Assert.Equal(StatusCode.Aborted, exception.StatusCode);
            Assert.Equal("Status(StatusCode=Aborted, Detail=\"Conccurency Exception.\")", exception.Message);
            KundeDto kunde = _target.Get(new KundeRequest { Id = 2 });

            //assert
            CompareKundenDtos(kunde, 2, "Bar", "Foo", Timestamp.FromDateTime(new DateTime(2000, 01, 01, 0,0,0, DateTimeKind.Utc)));
        }
    }
}
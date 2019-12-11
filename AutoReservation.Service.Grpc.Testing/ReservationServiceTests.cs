using System;
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
    public class ReservationServiceTests
        : ServiceTestBase
    {
        private readonly ReservationService.ReservationServiceClient _target;
        private readonly AutoService.AutoServiceClient _autoClient;
        private readonly KundeService.KundeServiceClient _kundeClient;

        public ReservationServiceTests(ServiceTestFixture serviceTestFixture)
            : base(serviceTestFixture)
        {
            _target = new ReservationService.ReservationServiceClient(Channel);
            _autoClient = new AutoService.AutoServiceClient(Channel);
            _kundeClient = new KundeService.KundeServiceClient(Channel);
        }

        [Fact]
        public async Task GetReservationenTest()
        {
            // arrange
            ReservationenDto reservationen = _target.GetAll(new Empty());
            RepeatedField<ReservationDto> reservationenDtos = reservationen.Reservationen;
            
            // act
            
            // assert
            Assert.Equal(4,reservationenDtos.Count);
            CompareReservationDtos(reservationenDtos[0], 1,
                Timestamp.FromDateTime(new DateTime(2020, 01, 10, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 01, 20, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 1 } ),
                _autoClient.Get(new AutoRequest { Id = 1} ));
            CompareReservationDtos(reservationenDtos[1], 2,
                Timestamp.FromDateTime(new DateTime(2020, 05, 19, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 06, 19, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 1 } ),
                _autoClient.Get(new AutoRequest { Id = 2 } ));
            CompareReservationDtos(reservationenDtos[2], 3,
                Timestamp.FromDateTime(new DateTime(2020, 01, 10, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 01, 20, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 2 } ),
                _autoClient.Get(new AutoRequest { Id = 2 } ));
            CompareReservationDtos(reservationenDtos[3], 4,
                Timestamp.FromDateTime(new DateTime(2020, 01, 10, 0,0,0, DateTimeKind.Utc)),
                Timestamp.FromDateTime(new DateTime(2020, 01, 20, 0,0,0, DateTimeKind.Utc)),
                _kundeClient.Get(new KundeRequest {Id = 3 } ),
                _autoClient.Get(new AutoRequest {Id = 3 } ));
        }
        
        private void CompareReservationDtos(ReservationDto reservation, int reservationsNr, Timestamp von, Timestamp bis, KundeDto kundeDto, AutoDto autoDto) {
            Assert.Equal(reservationsNr, reservation.ReservationsNr);
            Assert.Equal(von, reservation.Von);
            Assert.Equal(bis, reservation.Bis);
            Assert.Equal(kundeDto, reservation.Kunde);
            Assert.Equal(autoDto, reservation.Auto);
        } 

        [Fact]
        public async Task GetReservationByIdTest()
        {
            // arrange
            ReservationDto reservation1 = _target.Get(new ReservationRequest { Id = 1 } );
            ReservationDto reservation2 = _target.Get(new ReservationRequest { Id = 2 } );
            ReservationDto reservation3 = _target.Get(new ReservationRequest { Id = 3 } );
            ReservationDto reservation4 = _target.Get(new ReservationRequest { Id = 4 } );
            
            // act
            
            // assert
            CompareReservationDtos(reservation1, 1,
                Timestamp.FromDateTime(new DateTime(2020, 01, 10, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 01, 20, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 1 } ),
                _autoClient.Get(new AutoRequest { Id = 1} ));
            CompareReservationDtos(reservation2, 2,
                Timestamp.FromDateTime(new DateTime(2020, 05, 19, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 06, 19, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 1 } ),
                _autoClient.Get(new AutoRequest { Id = 2 } ));
            CompareReservationDtos(reservation3, 3,
                Timestamp.FromDateTime(new DateTime(2020, 01, 10, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 01, 20, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 2 } ),
                _autoClient.Get(new AutoRequest { Id = 2 } ));
            CompareReservationDtos(reservation4, 4,
                Timestamp.FromDateTime(new DateTime(2020, 01, 10, 0,0,0, DateTimeKind.Utc)),
                Timestamp.FromDateTime(new DateTime(2020, 01, 20, 0,0,0, DateTimeKind.Utc)),
                _kundeClient.Get(new KundeRequest {Id = 3 } ),
                _autoClient.Get(new AutoRequest {Id = 3 } ));
        }

        [Fact]
        public async Task GetReservationByIdWithIllegalIdTest()
        {
            // arrange
            RpcException exception = Assert.Throws<RpcException>(() => _target.Get(new ReservationRequest {Id = 5}));

            // act

            // assert
            Assert.Equal(StatusCode.OutOfRange, exception.StatusCode);
            Assert.Equal("Status(StatusCode=OutOfRange, Detail=\"Id couldn't be found.\")", exception.Message);
        }

        [Fact]
        public async Task InsertReservationTest()
        {
            // arrange
            Timestamp von = Timestamp.FromDateTime(new DateTime(2021, 07, 15, 0,0,0, DateTimeKind.Utc));
            Timestamp bis = Timestamp.FromDateTime(new DateTime(2021, 07, 17, 0,0,0, DateTimeKind.Utc));
            KundeDto kundeDto = _kundeClient.Get(new KundeRequest { Id = 4 } );
            AutoDto autoDto = _autoClient.Get(new AutoRequest { Id = 4 } );
            
            ReservationDto reservation = new ReservationDto();
            reservation.Von = von;
            reservation.Bis = bis;
            reservation.Kunde = kundeDto;
            reservation.Auto = autoDto;
            
            // act
            ReservationDto reservationDto = _target.Insert(reservation);
            ReservationDto reservation1 = _target.Get(new ReservationRequest {Id = reservationDto.ReservationsNr});

            // assert
            CompareReservationDtos(reservation1, reservationDto.ReservationsNr, von, bis, kundeDto, autoDto);
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            // arrange
            int reservationDeleteReservationNr = 1;
            ReservationDto reservation = _target.Get(new ReservationRequest {Id = reservationDeleteReservationNr});
            
            // act
            _target.Delete(reservation);
            
            // assert
            RpcException exception =
                Assert.Throws<RpcException>(() => _target.Get(new ReservationRequest() {Id = reservationDeleteReservationNr}));
            Assert.Equal(StatusCode.OutOfRange, exception.StatusCode);
            Assert.Equal("Status(StatusCode=OutOfRange, Detail=\"Id couldn't be found.\")", exception.Message);
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            // arrange
            int reservationUpdateReservationNr = 2;
            Timestamp reservationUpdateVon = Timestamp.FromDateTime(new DateTime(2020, 03, 02, 0,0,0, DateTimeKind.Utc));
            Timestamp reservationUpdateBis = Timestamp.FromDateTime(new DateTime(2020, 03, 04, 0,0,0, DateTimeKind.Utc));
            
            ReservationDto reservationUpdate = _target.Get(new ReservationRequest {Id = reservationUpdateReservationNr});
            reservationUpdate.Von = reservationUpdateVon;
            reservationUpdate.Bis = reservationUpdateBis;
            
            // act
            _target.Update(reservationUpdate);
            ReservationDto reservationUpdated =
                _target.Get(new ReservationRequest {Id = reservationUpdateReservationNr});

            // assert
            CompareReservationDtos(reservationUpdated, reservationUpdateReservationNr,
                reservationUpdateVon, 
                reservationUpdateBis, 
                _kundeClient.Get(new KundeRequest { Id = 1 } ),
                _autoClient.Get(new AutoRequest { Id = 2 } ));
        }

        [Fact]
        public async Task UpdateReservationWithOptimisticConcurrencyTest()
        {
            // arrange
            ReservationDto reservation1 = _target.Get(new ReservationRequest {Id = 1});
            reservation1.Von = Timestamp.FromDateTime(new DateTime(2020, 10, 21, 0,0,0, DateTimeKind.Utc));
            reservation1.Bis = Timestamp.FromDateTime(new DateTime(2020, 10, 23, 0,0,0, DateTimeKind.Utc));

            ReservationDto reservation2 = _target.Get(new ReservationRequest {Id = 1});
            reservation2.Von = Timestamp.FromDateTime(new DateTime(2020, 10, 24, 0,0,0, DateTimeKind.Utc));
            reservation2.Bis = Timestamp.FromDateTime(new DateTime(2020, 10, 26, 0,0,0, DateTimeKind.Utc));
            
            //act
            _target.Update(reservation1);
            RpcException exception = Assert.Throws<RpcException>(() => _target.Update(reservation2));
            Assert.Equal(StatusCode.Aborted, exception.StatusCode);
            Assert.Equal("Status(StatusCode=Aborted, Detail=\"Conccurency Exception\")", exception.Message);

            //assert
            ReservationDto reservation = _target.Get(new ReservationRequest {Id = 2});
            CompareReservationDtos(reservation, 2,
                Timestamp.FromDateTime(new DateTime(2020, 03, 10, 0,0,0, DateTimeKind.Utc)), 
                Timestamp.FromDateTime(new DateTime(2020, 03, 15, 0,0,0, DateTimeKind.Utc)), 
                _kundeClient.Get(new KundeRequest { Id = 1 } ),
                _autoClient.Get(new AutoRequest { Id = 2 } ));
        }

        [Fact]
        public async Task InsertReservationWithInvalidDateRangeTest()
        {
            // arrange
            Timestamp von = Timestamp.FromDateTime(new DateTime(2020,10,04, 12, 00, 00, DateTimeKind.Utc));
            Timestamp bis = Timestamp.FromDateTime(new DateTime(2020, 10, 05, 11, 59, 59, DateTimeKind.Utc));
            KundeDto kundeDto = _kundeClient.Get(new KundeRequest { Id = 4 } );
            AutoDto autoDto = _autoClient.Get(new AutoRequest { Id = 4 } );
            
            ReservationDto reservation = new ReservationDto();
            reservation.Von = von;
            reservation.Bis = bis;
            reservation.Kunde = kundeDto;
            reservation.Auto = autoDto;
            
            // act - assert
            RpcException exception = Assert.Throws<RpcException>(() => _target.Insert(reservation));
            Assert.Equal(StatusCode.FailedPrecondition, exception.StatusCode);
            Assert.Equal("Status(StatusCode=FailedPrecondition, Detail=\"From-To must be at least 24 hours apart\")", exception.Message);
        }

        [Fact]
        public async Task InsertReservationWithAutoNotAvailableTest()
        {
            // arrange
            ReservationDto reservation = new ReservationDto() {
                Von = Timestamp.FromDateTime(new DateTime(2020,01,11, 0,0,0, DateTimeKind.Utc)),
                Bis = Timestamp.FromDateTime(new DateTime(2020, 01, 13, 0,0,0, DateTimeKind.Utc)),
                Kunde = _kundeClient.Get(new KundeRequest { Id = 4 } ),
                Auto = _autoClient.Get(new AutoRequest { Id = 4 } )
            };

            // act - assert
            RpcException exception = Assert.Throws<RpcException>(() => _target.Insert(reservation));
            Assert.Equal(StatusCode.FailedPrecondition, exception.StatusCode);
            Assert.Equal("Status(StatusCode=FailedPrecondition, Detail=\"Car is not available\")", exception.Message);
        }

        [Fact]
        public async Task UpdateReservationWithInvalidDateRangeTest()
        {
            // arrange
            ReservationDto reservationUpdate = _target.Get(new ReservationRequest {Id = 3});
            reservationUpdate.Von = Timestamp.FromDateTime(new DateTime(2020, 10, 04, 12, 00, 00, DateTimeKind.Utc));
            reservationUpdate.Bis = Timestamp.FromDateTime(new DateTime(2020, 10, 05, 11, 59, 59, DateTimeKind.Utc));
            
            // act - assert
            RpcException exception = Assert.Throws<RpcException>(() => _target.Update(reservationUpdate));
            Assert.Equal(StatusCode.FailedPrecondition, exception.StatusCode);
            Assert.Equal("Status(StatusCode=FailedPrecondition, Detail=\"From-To must be at least 24 hours apart\")", exception.Message);
        }

        [Fact]
        public async Task UpdateReservationWithAutoNotAvailableTest()
        {
            // arrange
            ReservationDto reservation = new ReservationDto() {
                ReservationsNr = 3,
                Von = Timestamp.FromDateTime(new DateTime(2020,01,11, 0,0,0, DateTimeKind.Utc)),
                Bis = Timestamp.FromDateTime(new DateTime(2020, 01, 13, 0,0,0, DateTimeKind.Utc)),
                Kunde = _kundeClient.Get(new KundeRequest { Id = 4 } ),
                Auto = _autoClient.Get(new AutoRequest { Id = 4 } )
            };

            // act - assert
            RpcException exception = Assert.Throws<RpcException>(() => _target.Update(reservation));
            Assert.Equal(StatusCode.FailedPrecondition, exception.StatusCode);
            Assert.Equal("Status(StatusCode=FailedPrecondition, Detail=\"Car is not available\")", exception.Message);
        }

        [Fact]
        public async Task CheckAvailabilityIsTrueTest()
        {
            ReservationDto reservation = new ReservationDto() {
                ReservationsNr = 3,
                Von = Timestamp.FromDateTime(new DateTime(2020,01,21, 0,0,0, DateTimeKind.Utc)),
                Bis = Timestamp.FromDateTime(new DateTime(2020, 01, 23, 0,0,0, DateTimeKind.Utc)),
                Kunde = _kundeClient.Get(new KundeRequest { Id = 4 } ),
                Auto = _autoClient.Get(new AutoRequest { Id = 4 } )
            };

            // act
            AutoAvailableResponse result = _target.IsCarAvailable(reservation);
            
            // assert
            Assert.True(result.Available);
     
        }

        [Fact]
        public async Task CheckAvailabilityIsFalseTest()
        {
            ReservationDto reservation = new ReservationDto() {
                ReservationsNr = 3,
                Von = Timestamp.FromDateTime(new DateTime(2020,01,11, 0,0,0, DateTimeKind.Utc)),
                Bis = Timestamp.FromDateTime(new DateTime(2020, 01, 13, 0,0,0, DateTimeKind.Utc)),
                Kunde = _kundeClient.Get(new KundeRequest { Id = 4 } ),
                Auto = _autoClient.Get(new AutoRequest { Id = 4 } )
            };

            // act
            AutoAvailableResponse result = _target.IsCarAvailable(reservation);
            
            // assert
            Assert.False(result.Available);
        }
    }
}
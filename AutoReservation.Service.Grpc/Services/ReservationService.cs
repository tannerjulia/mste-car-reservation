using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal.Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class ReservationService : Grpc.ReservationService.ReservationServiceBase
    {
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
        }

        public override async Task<ReservationenDto> GetAll(Empty request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            List<Reservation> allReservations = await manager.GetAll();
            List<ReservationDto> reservationDtos = allReservations.ConvertToDtos();
            ReservationenDto reservationenDto = new ReservationenDto();
            reservationDtos.ForEach(reservationDto => reservationenDto.Reservationen.Add(reservationDto));
            return reservationenDto;
        }

        public override async Task<ReservationDto> Get(ReservationRequest request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            Reservation reservation = await manager.Get(request.Id);
            if (reservation == null)
            {
                throw new RpcException(new Status(
                    StatusCode.OutOfRange, "Id couldn't be found."
                ));
            }
            return reservation.ConvertToDto();
        }

        public override async Task<ReservationDto> Insert(ReservationDto request, ServerCallContext context)
        {
            try
            {
                ReservationManager manager = new ReservationManager();
                Reservation reservation = request.ConvertToEntity();
                Reservation newreservation = await manager.Insert(reservation);
                return newreservation.ConvertToDto();
            }
            catch (InvalidDateRangeException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "From-To must be at least 24 hours apart"
                ), exception.ToString());
            }
            catch (AutoUnavailableException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Car is not available"
                ), exception.ToString());
            }
        }

        public override async Task<Empty> Update(ReservationDto request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            Reservation reservation = request.ConvertToEntity();
            try
            {
                await manager.Update(reservation);
            }
            catch (OptimisticConcurrencyException<Reservation> exception)
            {
                throw new RpcException(new Status(
                    StatusCode.Aborted,
                    "Conccurency Exception"
                ), exception.ToString());
            }
            catch (InvalidDateRangeException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "From-To must be at least 24 hours apart"
                ), exception.ToString());
            }
            catch (AutoUnavailableException exception)
            {
                throw new RpcException(new Status(
                    StatusCode.FailedPrecondition,
                    "Car is not available"
                ), exception.ToString());
            }
            return new Empty();
        }

        public override async Task<Empty> Delete(ReservationDto request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            Reservation reservation = request.ConvertToEntity();
            await manager.Delete(reservation);
            return new Empty();
        }

        public override async Task<AutoAvailableResponse> IsCarAvailable(ReservationDto request, ServerCallContext context)
        {
            ReservationManager manager = new ReservationManager();
            Reservation reservation = request.ConvertToEntity();
            try
            {
                bool isCarAvailable = await manager.IsCarAvailable(reservation);
                return new AutoAvailableResponse { Available = isCarAvailable };
            } catch 
            {
                return new AutoAvailableResponse {Available = false};
            }
        }
    }
}

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
    internal class AutoService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<AutoService> _logger;

        public AutoService(ILogger<AutoService> logger)
        {
            _logger = logger;
        }

        public override async Task<AutosDto> GetAll(Empty request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            List<Auto> allCars = await manager.GetAll();
            List<AutoDto> dtos = allCars.ConvertToDtos();
            AutosDto autosDto = new AutosDto();
            dtos.ForEach(autoDto => autosDto.Cars.Add(autoDto));
            return autosDto;
        }
        public override async Task<AutoDto> Get(AutoRequest request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            Auto car = await manager.Get(request.Id);
            return car.ConvertToDto();
        }

        public override async Task<Empty> Insert(AutoDto request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            Auto car = request.ConvertToEntity();
            await manager.Insert(car);
            return new Empty();
        }

        public override async Task<Empty> Update(AutoDto request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            Auto car = request.ConvertToEntity();
            try
            {
                await manager.Update(car);
            }
            catch (OptimisticConcurrencyException<Auto> exception) //TODO error handling evtl. noch nicht korrekt
            {
                throw new RpcException(new Status(
                    StatusCode.Aborted,
                    "Conccurency Exception."
                    ), exception.ToString());
            }
            return new Empty();
        }

        public override async Task<Empty> Delete(AutoDto request, ServerCallContext context)
        {
            AutoManager manager = new AutoManager();
            Auto car = request.ConvertToEntity();
            await manager.Delete(car);
            return new Empty();
        }
    }
}

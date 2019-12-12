using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Channels;
using AutoReservation.Service.Grpc;
using AutoUi.Core.Services;
using AutoUi.Core.ViewModels;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace AutoUi.Service
{
    public class DataProviderService : IModelDataService
    {

        private readonly AutoService.AutoServiceClient autoServiceClient;
        private readonly KundeService.KundeServiceClient kundeServiceClient;

        public DataProviderService()
        {
            GrpcChannel channel = GrpcChannel.ForAddress(
                "https://localhost:50001",
                new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(
                        new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        }
                    )
                });
            autoServiceClient = new AutoService.AutoServiceClient(channel);
            kundeServiceClient = new KundeService.KundeServiceClient(channel);
        }

        public ObservableCollection<AutoVm> GetCars()
        {
            RepeatedField<AutoDto> autosDto = autoServiceClient.GetAll(new Empty()).Cars;
            ObservableCollection<AutoVm> cars = new ObservableCollection<AutoVm>();
            foreach (AutoDto autoDto in autosDto)
            {
                AutoVm autoVm = new AutoVm();
                PoorMansObjectCloner.CopyProperties(autoDto, autoVm);
                cars.Add(autoVm);
            }

            return cars;
        }

        public ObservableCollection<CustomerVm> GetCustomers()
        {
            RepeatedField<KundeDto> dtos = kundeServiceClient.GetAll(new Empty()).Customers;
            ObservableCollection<CustomerVm> customers = new ObservableCollection<CustomerVm>();
            foreach (KundeDto dto in dtos)
            {
                CustomerVm customerVm = new CustomerVm();
                PoorMansObjectCloner.CopyProperties(dto, customerVm);
                customers.Add(customerVm);
            }

            return customers;
        }
    }
}
using System.Collections.ObjectModel;
using AutoUi.Core.ViewModels;

namespace AutoUi.Core.Services
{
    public interface IModelDataService
    {
        ObservableCollection<AutoVm> GetCars();
        ObservableCollection<CustomerVm> GetCustomers();
    }

}
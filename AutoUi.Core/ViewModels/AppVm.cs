using System.Linq;
using System.Windows.Input;
using AutoUi.Core.Commands;
using AutoUi.Core.Services;

namespace AutoUi.Core.ViewModels
{
    // Unser App ViewModel ist diese Woche noch im WPF-Projekt,
    // da wir von hier aus ja direkt WPF-Objekte erstellen
    // (resp. Fenster erstellen und öffnen).
    // Nächste Woche besprechen wir noch, wie wir das noch
    // besser lösen können (mit Interfaces, konkreten Implementationen
    // davon und Dependency Injection)
    public class AppVm : BindableBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsActual => !IsLoading;

        public AutoListVm AutoListModel { get; set; }
        public AutoVm DemoAuto { get; set; }
        public CustomerVm DemoCustomer { get; set; }

        // Die Commands brauchen wir nur zu lesen
        // (werden nur im Konstruktor geschrieben, daher 
        // können wir auf einen Setter verzichten)
        public ICommand ShowAutoListCommand { get; }
        public ICommand EditAutoCommand { get; }
        public ICommand EditCustomerCommand { get; }

        public INavigationService NavigationService { get; }

        public AppVm(IModelDataService modelDataService, INavigationService navigationService)
        {
            NavigationService = navigationService;

            // Ohne Parameter, daher direkt RelayCommand
            ShowAutoListCommand = new RelayCommand(ShowAutoList);

            // Mit dem jeweiligen ViewModel als Command Parameter,
            // daher RelayCommand<T>
            EditAutoCommand = new RelayCommand<AutoVm>(EditAuto);
            EditCustomerCommand = new RelayCommand<CustomerVm>(EditCustomer);

            // unsere ViewModels (mit Beispieldaten)
            AutoListModel = new AutoListVm(modelDataService);
            DemoAuto = modelDataService.GetCars().First();
            DemoCustomer = modelDataService.GetCustomers().First();

        }


        public void ShowAutoList()
        {
            // Hier greifen wir direkt auf eine "View" zu,
            // was wir eigentlich in einem ViewModel nicht wollen
            // -> vgl. Stoff Block 6 nächste Woche
            AutoListModel.RefreshCars();
            NavigationService.Display(AutoListModel);
        }

        public void EditAuto(AutoVm auto)
        {
            // Hier greifen wir direkt auf eine "View" zu,
            // was wir eigentlich in einem ViewModel nicht wollen
            // -> vgl. Stoff Block 6 nächste Woche
            NavigationService.Display(auto);
        }

        public void EditCustomer(CustomerVm customer)
        {
            // Hier greifen wir direkt auf eine "View" zu,
            // was wir eigentlich in einem ViewModel nicht wollen
            // -> vgl. Stoff Block 6 nächste Woche
            NavigationService.Display(customer);
        }
    }
}

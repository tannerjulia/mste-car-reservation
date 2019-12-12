using AutoUi.Core.ViewModels;

namespace AutoUi.Core.Services
{
    public interface INavigationService
    {
        void Display<T>(T viewmodel);
    }
}
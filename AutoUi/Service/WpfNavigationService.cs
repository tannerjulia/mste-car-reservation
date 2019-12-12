using System;
using System.Collections.Generic;
using AutoUi.Core.Services;
using AutoUi.Core.ViewModels;
using AutoUi.Views;

namespace AutoUi.Service
{
    public class WpfNavigationService : INavigationService
    {
        public readonly Dictionary<Type, Action<object>> WindowFactory = 
            new Dictionary<Type, Action<object>>();

        public WpfNavigationService()
        {
            WindowFactory.Add(typeof(AutoListVm), vm => AutoListWindow.Display(vm as AutoListVm));
            WindowFactory.Add(typeof(AutoVm), vm => AutoWindow.Display(vm as AutoVm)); 
            WindowFactory.Add(typeof(CustomerVm), vm => CustomerWindow.Display(vm as CustomerVm));
        }

        public void Display<T>(T viewmodel)
        {
            var factoryAction = WindowFactory[typeof(T)]; 
            factoryAction(viewmodel);
        }
    }
}
using System.Diagnostics;
using System.Windows;
using AutoUi.Core.Services;
using AutoUi.Core.ViewModels;

namespace AutoUi.Views
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerVm Model { get; set; }

        public CustomerWindow(CustomerVm model)
        {
            InitializeComponent();

            // falls kein Model übergeben wurde, erstellen wir einfach eines:
            Model = model ?? new CustomerVm();

            DataContext = Model;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Achtung: folgende Zeile wirft eine Exception, falls man
            // das Fenster nicht mit der .ShowDialog()-Methode, sondern
            // nur mit der normalen .Show()-Methode anzeigt
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Achtung: folgende Zeile wirft eine Exception, falls man
            // das Fenster nicht mit der .ShowDialog()-Methode, sondern
            // nur mit der normalen .Show()-Methode anzeigt
            DialogResult = false;
        }

        public static bool Display(CustomerVm model)
        {
            // damit wir beim Abbrechen nicht aus Versehen etwas
            // übernehmen, das wir nicht wollen, arbeiten wir
            // temporär mit einer Kopie
            var vm = PoorMansObjectCloner.Clone<CustomerVm>(model);

            var win = new CustomerWindow(vm);

            // wir zeigen das Fenster als Dialogfenster (blockierend!) an
            if (win.ShowDialog() != true)
            {
                // Abbrechen ...
                Debug.WriteLine("Bearbeitung des Kunden abgebrochen");
                return false;
            }

            // Ok geklickt... 
            Debug.WriteLine("Bearbeitung des Kunden beendet");

            // nun wollen wir die geänderten Properties zurück ins
            // Originalobjekt übernehmen
            PoorMansObjectCloner.CopyProperties(vm, model);

            return true;
        }
    }
}

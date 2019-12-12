using System.Diagnostics;
using System.Windows;
using AutoUi.Core.Services;
using AutoUi.Core.ViewModels;

namespace AutoUi.Views
{
    /// <summary>
    /// Interaction logic for AutoWindow.xaml
    /// </summary>
    public partial class AutoWindow : Window
    {
        public AutoVm Model { get; set; }

        public AutoWindow(AutoVm model)
        {
            InitializeComponent();

            Model = model;

            DataContext = Model;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Helper-Methode zum einfachen Anzeigen des Fensters
        /// </summary>
        /// <param name="model">Das ViewModel, welches angezeigt werden soll</param>
        /// <returns>true, falls der User Ok geklickt hat</returns>
        public static bool Display(AutoVm model)
        {
            // damit wir beim Abbrechen nicht aus Versehen etwas
            // übernehmen, das wir nicht wollen, arbeiten wir
            // temporär mit einer Kopie
            var vm = PoorMansObjectCloner.Clone<AutoVm>(model);

            var win = new AutoWindow(vm);

            // wir zeigen das Fenster als Dialogfenster (blockierend!) an
            if (win.ShowDialog() != true)
            {
                // Abbrechen ...
                Debug.WriteLine("Bearbeitung des Autos abgebrochen");
                return false;
            }

            // Ok geklickt... 
            Debug.WriteLine("Bearbeitung des Autos beendet");

            // nun wollen wir die geänderten Properties zurück ins
            // Originalobjekt übernehmen
            PoorMansObjectCloner.CopyProperties(vm, model);

            return true;
        }
    }
}

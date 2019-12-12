using System;
using System.Windows;
using AutoUi.ViewModels;

namespace AutoUi.Views
{
    /// <summary>
    /// Interaction logic for ConfirmBox.xaml
    /// </summary>
    public partial class ConfirmBox : Window
    {
        public ConfirmBox(string winTitle, string title, string msg, string okButtonCaption, string cancelButtonCaption)
        {
            InitializeComponent();

            // Instanz unseres ViewModels erzeugen und Werte initialisieren:
            var vm = new ConfirmBoxVm()
            {
                WindowTitle = winTitle,
                Title = title,
                Message = msg,
                OkButtonCaption = okButtonCaption,
                CancelButtonCaption = cancelButtonCaption
            };

            DataContext = vm;

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public static void Display(string winTitle, string title, string msg, 
            string okButtonCaption = "OK",
            string cancelButtonCaption = "Abbrechen",
            Action okCallback = null)
        {
            // Fenster (unsichtbar) erstellen
            var win = new ConfirmBox(winTitle, title, msg, okButtonCaption, cancelButtonCaption);

            // und als Dialogfenster (d.h. blockierend!) anzeigen
            var result = win.ShowDialog();
            if (result != true)
                return;

            okCallback?.Invoke();

            // Syntax mit ? ist gleichbedeutend mit folgendem, deutlich längerem Code:
            //if (okCallback != null)
            //{
            //    okCallback();
            //}
        }
    }
}

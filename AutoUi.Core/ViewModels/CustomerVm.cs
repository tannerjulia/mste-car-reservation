using System;

namespace AutoUi.Core.ViewModels
{
    public class CustomerVm : BindableBase
    {
        private string _vorname;

        public string Vorname
        {
            // hier mit "alter" Syntax
            get { return _vorname; }
            set { SetProperty(ref _vorname, value, null /* wird vom Compiler eingesetzt */, nameof(Name), nameof(HatName), nameof(IstLeer)); }
        }


        private string _nachname;

        public string Nachname
        {
            // ab hier mit kürzerer Lambda-Syntax
            get => _nachname;
            set => SetProperty(ref _nachname, value, null /* tx, Compiler :-) */, nameof(Name), nameof(HatName), nameof(IstLeer));
        }


        // berechnete Properties:
        public string Name => $"{Vorname} {Nachname}";
        public bool HatName => !string.IsNullOrEmpty(Nachname) || !string.IsNullOrEmpty(Vorname);
        public bool IstLeer => !HatName;


        private DateTime _geburtstag;

        public DateTime Geburtstag
        {
            get => _geburtstag;
            set => SetProperty(ref _geburtstag, value);
        }

        public CustomerVm()
        {
            // ein nicht initialisiertes Datum ist 01.01.0001, das wollen wir nicht.
            // wir starten daher lieber mit dem heutigen Datum:
            Geburtstag = DateTime.Now;
        }
    }
}
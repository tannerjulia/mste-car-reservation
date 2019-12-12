namespace AutoUi.ViewModels
{
    // hier brauchen wir kein INotifyPropertyChanged, da 
    // sich während der Anzeige der ConfirmBox ja nichts ändert,
    // d.h. es ist auch nicht nötig, über die (nicht stattfindenden)
    // Änderungen informiert zu werden...
    public class ConfirmBoxVm
    {
        public string WindowTitle { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public string OkButtonCaption { get; set; } = "OK";
        public string CancelButtonCaption { get; set; } = "Abbrechen";
    }
}

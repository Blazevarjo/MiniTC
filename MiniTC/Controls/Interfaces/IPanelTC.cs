using System.Collections.ObjectModel;

namespace MiniTC.Controls.Interfaces
{
    interface IPanelTC
    {
        string CurrentPath { get; set; }
        ObservableCollection<string> AvailableDrives { get; set; }
        ObservableCollection<string> CurrentPathContent { get; set; }
    }
}

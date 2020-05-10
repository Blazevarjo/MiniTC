using System;

namespace MiniTC.ViewModel
{
    using BaseClass;
    using System.Windows.Input;

    class MainViewModel : ViewModelBase
    {
        private PanelTCViewModel _source;
        public PanelTCViewModel Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(nameof(Source)); }
        }

        private PanelTCViewModel _target;
        public PanelTCViewModel Target
        {
            get { return _target; }
            set { _target = value; OnPropertyChanged(nameof(Target)); }
        }

        public MainViewModel()
        {
            Source = new PanelTCViewModel();
            Target = new PanelTCViewModel();
            
            Copy = new RelayCommand(CopyExecute, CopyCanExecute);
        }

        public ICommand Copy { get; set; }

        private void CopyExecute(object obj)
        {
            Console.WriteLine(Source.SelectedDrive);
        }

        private bool CopyCanExecute(object obj)
        {
            return Source.SelectedPath == null;
        }
    }
}

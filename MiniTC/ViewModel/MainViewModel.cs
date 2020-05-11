using System;

namespace MiniTC.ViewModel
{
    using BaseClass;
    using System.Windows.Input;

    class MainViewModel : ViewModelBase
    {
        #region Properties and fields
        private PanelTCViewModel _leftPanel;
        public PanelTCViewModel LeftPanel
        {
            get { return _leftPanel; }
            set { _leftPanel = value; OnPropertyChanged(nameof(LeftPanel)); }
        }

        private PanelTCViewModel _rightPanel;
        public PanelTCViewModel RightPanel
        {
            get { return _rightPanel; }
            set { _rightPanel = value; OnPropertyChanged(nameof(RightPanel)); }
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            _leftPanel = new PanelTCViewModel();
            _rightPanel = new PanelTCViewModel();

            Copy = new RelayCommand(CopyExecute, CopyCanExecute);
            LeftSelectionChange = new RelayCommand(LeftSelectionChangeExecute, arg => true);
            RightSelectionChange = new RelayCommand(RightSelectionChangeExecute, arg => true);
        }
        #endregion

        #region Commands
        public ICommand Copy { get; set; }
        public ICommand LeftSelectionChange { get; set; }
        public ICommand RightSelectionChange { get; set; }

        private void CopyExecute(object obj)
        {
            Console.WriteLine("source : " + LeftPanel.SelectedPath);
            Console.WriteLine("target : " + RightPanel.SelectedPath);
        }

        private bool CopyCanExecute(object obj)
        {
            return LeftPanel.SelectedPath != null || RightPanel.SelectedPath != null;
        }

        private void LeftSelectionChangeExecute(object obj)
        {
            if(RightPanel.SelectedPath !=null && LeftPanel.SelectedPath !=null)
                RightPanel.SelectedPath = null;
        }

        private void RightSelectionChangeExecute(object obj)
        {
            if(LeftPanel.SelectedPath !=null && RightPanel.SelectedPath != null)
                LeftPanel.SelectedPath = null;
        }
        #endregion
    }
}

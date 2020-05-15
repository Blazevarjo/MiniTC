namespace MiniTC.ViewModel
{
    using BaseClass;
    using MiniTC.Properties;
    using Model;
    using System.IO;
    using System.Windows.Input;

    class MainViewModel : ViewModelBase
    {
        #region Properties and fields
        private CopyingModel _model;
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
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US"); // Default is English
            _leftPanel = new PanelTCViewModel();
            _rightPanel = new PanelTCViewModel();
            _model = new CopyingModel();
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
            string source = "";
            string target = "";
            if (LeftPanel.SelectedPath != null)
            {
                source = Path.Combine(LeftPanel.CurrentPath, LeftPanel.GetCorrectSelectedPath());
                target = Path.GetFullPath(RightPanel.CurrentPath);
            }
            else if (RightPanel.SelectedPath != null)
            {
                source = Path.Combine(RightPanel.CurrentPath, RightPanel.GetCorrectSelectedPath());
                target = Path.GetFullPath(LeftPanel.CurrentPath);
            }

           _model.Copy(source, target); // Model usage


            UpdateCurrentPathContents(); // Updating view
        }

        private bool CopyCanExecute(object obj)
        {
            if (LeftPanel.SelectedPath == null && RightPanel.SelectedPath == null) return false;
            if (LeftPanel.SelectedPath == Resources.ParentDirectory || RightPanel.SelectedPath == Resources.ParentDirectory) return false;
            return true;
        }

        private void LeftSelectionChangeExecute(object obj)
        {
            if (RightPanel.SelectedPath != null && LeftPanel.SelectedPath != null)
                RightPanel.SelectedPath = null;
        }

        private void RightSelectionChangeExecute(object obj)
        {
            if (LeftPanel.SelectedPath != null && RightPanel.SelectedPath != null)
                LeftPanel.SelectedPath = null;
        }
        #endregion

        #region Auxiliary functions
        private void UpdateCurrentPathContents()
        {
            LeftPanel.UpdateCurrentPathContent();
            RightPanel.UpdateCurrentPathContent();
        }
        #endregion
    }
}

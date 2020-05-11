using System;

namespace MiniTC.ViewModel
{
    using BaseClass;
    using MiniTC.Properties;
    using System.IO;
    using System.Linq;
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
            string source = "";
            string target = "";
            if(LeftPanel.SelectedPath != null)
            {
                source = Path.Combine(LeftPanel.CurrentPath, LeftPanel.GetCorrectSelectedPath());
                target = Path.GetFullPath(RightPanel.CurrentPath);
            }
            else if(RightPanel.SelectedPath != null)
            {
                source = Path.Combine(RightPanel.CurrentPath,RightPanel.GetCorrectSelectedPath());
                target = Path.GetFullPath(LeftPanel.CurrentPath);
            }
            Console.WriteLine($"source: {source}\ntarget: {target}\n");

            //checking if path is directory or file
            var attribute = File.GetAttributes(source);
            if (attribute.HasFlag(FileAttributes.Directory))
            {
                foreach (var  item in Directory.GetFiles(target))
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                if (Directory.GetFiles(target).Select(x => Path.GetFileName(x)).Contains(Path.GetFileName(source)))
                {
                    int count = Directory.GetFiles(target).Select(x => Path.GetFileName(x)).Where(x => x.StartsWith(Path.GetFileNameWithoutExtension(source))).Count();
                    string fileName = Path.GetFileNameWithoutExtension(source) + " - " + Resources.FileCopy + count + Path.GetExtension(source);
                    target = Path.Combine(target, fileName);
                }
                else
                {
                    target += Path.GetFileName(source);
                }
                File.Copy(source, target);
            }
            UpdateCurrentPathContents();
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

        private void UpdateCurrentPathContents()
        {
            LeftPanel.UpdateCurrentPathContent();
            RightPanel.UpdateCurrentPathContent();
        }
    }
}

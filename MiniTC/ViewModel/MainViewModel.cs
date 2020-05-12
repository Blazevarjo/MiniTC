using System;

namespace MiniTC.ViewModel
{
    using BaseClass;
    using MiniTC.Properties;
    using System.Globalization;
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
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US"); // Default is English
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

            //checking if path is directory or file
            var attribute = File.GetAttributes(source);
            if (attribute.HasFlag(FileAttributes.Directory))
            {
                target = Path.Combine(target, Path.GetFileName(source));
                DirectoryCopy(source, target);
            }
            else
            {
                FileCopy(source, target);
            }
            UpdateCurrentPathContents();
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

        private void FileCopy(string source, string target)
        {
            if (Directory.GetFiles(target).Select(x => Path.GetFileName(x)).Contains(Path.GetFileName(source)))
            {
                int count = Directory.GetFiles(target).Select(x => Path.GetFileName(x)).Where(x => x.StartsWith(Path.GetFileNameWithoutExtension(source))).Count();
                string fileName = Path.GetFileNameWithoutExtension(source) + " - " + Resources.FileCopy + count + Path.GetExtension(source);
                target = Path.Combine(target, fileName);
            }
            else
            {
                target = Path.Combine(target, Path.GetFileName(source));
            }
            try
            {
                File.Copy(source, target);
            }
            catch (UnauthorizedAccessException) { return; }
        }

        private void DirectoryCopy(string source, string target)
        {
            var dir = new DirectoryInfo(source);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            DirectoryInfo[] dirs;
            try
            {
                dirs = dir.GetDirectories();
            }
            catch (UnauthorizedAccessException) { return; }

            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            else
            {
                int count = Directory.GetDirectories(Directory.GetDirectoryRoot(target)).Where(x => x.StartsWith(target)).Count();
                target = Path.Combine(Path.GetDirectoryName(target), Path.GetFileNameWithoutExtension(target) + " - " + Resources.FileCopy + count);
                Directory.CreateDirectory(target);
            }
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                string path = Path.Combine(target, file.Name);
                file.CopyTo(path);
            }
            foreach (var subdir in dirs)
            {
                string path = Path.Combine(target, subdir.Name);
                DirectoryCopy(subdir.FullName, path);
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MiniTC.ViewModel
{
    using MiniTC.Properties;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using ViewModel.BaseClass;

    class PanelTCViewModel : ViewModelBase
    {
        #region Properties and values
        private string _currentPath;
        public string CurrentPath
        {
            get { return _currentPath; }
            set { _currentPath = value; OnPropertyChanged(nameof(CurrentPath)); }
        }

        private ObservableCollection<string> _availableDrives;
        public ObservableCollection<string> AvailableDrives
        {
            get { return _availableDrives; }
            set { _availableDrives = value; OnPropertyChanged(nameof(AvailableDrives)); }
        }

        private ObservableCollection<string> _currentPathContent;
        public ObservableCollection<string> CurrentPathContent
        {
            get { return _currentPathContent; }
            set { _currentPathContent = value; OnPropertyChanged(nameof(CurrentPathContent)); }
        }

        private string _selectedPath;
        public string SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; OnPropertyChanged(nameof(SelectedPath)); }
        }

        private string _selectedDrive;
        public string SelectedDrive
        {
            get { return _selectedDrive; }
            set
            {
                _selectedDrive = value;
                OnPropertyChanged(nameof(SelectedDrive));
                CurrentPath = _selectedDrive;
                UpdateCurrentPathContent();
            }
        }
        #endregion

        #region Constructors
        public PanelTCViewModel()
        {
            UpdateAvailableDrives();
            CurrentPathContent = new ObservableCollection<string>();
            SelectedDrive = AvailableDrives.Any(x => x.Contains("C")) ? AvailableDrives.Where(x => x.Contains("C")).First() : AvailableDrives.First();

            DropDownOpen = new RelayCommand(DropDownOpenExecute, arg => true);
            ItemDoubleClick = new RelayCommand(ItemDoubleClickExecute, arg => true);
            ItemEnterKey = new RelayCommand(ItemEnterKeyExecute, ItemEnterKeyCanExecute);
        }
        #endregion

        #region Commands
        public ICommand DropDownOpen { get; set; }
        public ICommand ItemDoubleClick { get; set; }
        public ICommand ItemEnterKey { get; set; }

        private void DropDownOpenExecute(object obj) => UpdateAvailableDrives();
 
        private void ItemDoubleClickExecute(object obj) => EnterDirectory();
   
        private void ItemEnterKeyExecute(object obj) => EnterDirectory();
        private bool ItemEnterKeyCanExecute(object obj) => SelectedPath != null;
        #endregion

        #region Auxiliary functions
        private void EnterDirectory()
        {
            if (SelectedPath == null) return;
            if (SelectedPath.StartsWith(Resources.DriveSign))
            {
                CurrentPath = Path.Combine(CurrentPath, SelectedPath.Substring(4));
                UpdateCurrentPathContent();
            }
            else if (SelectedPath == Resources.ParentDirectory)
            {
                CurrentPath = Path.GetDirectoryName(CurrentPath);
                UpdateCurrentPathContent();
            }
        }
        private void UpdateAvailableDrives()
        {
            AvailableDrives = new ObservableCollection<string>(Directory.GetLogicalDrives().ToList());
        }

        // 2 Functions which eliminates chance of getting files or directories which we don't have access to
        private List<string> GetFiles(string path)
        {
            var files = new List<string>();
            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    files.Add(file);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            return files;
        }

        private List<string> GetDirectories(string path)
        {
            var directories = new List<string>();
            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    directories.Add(directory);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            return directories;
        }

        public void UpdateCurrentPathContent()
        {
            CurrentPathContent.Clear();
            if (!AvailableDrives.Contains(CurrentPath))
            {
                CurrentPathContent.Add(Resources.ParentDirectory);
            }
            foreach (var dir in GetDirectories(CurrentPath))
            {
                CurrentPathContent.Add(Resources.DriveSign + Path.GetFileName(dir));
            }
            foreach (var file in GetFiles(CurrentPath))
            {
                CurrentPathContent.Add(Path.GetFileName(file));
            }
        }

        public string GetCorrectSelectedPath() => SelectedPath.StartsWith(Resources.DriveSign) ? SelectedPath.Substring(Resources.DriveSign.Length) : SelectedPath;

        public void RaiseContentChange() => OnPropertyChanged(nameof(CurrentPathContent));
        
        #endregion
    }
}

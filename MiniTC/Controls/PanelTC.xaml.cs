using MiniTC.Controls.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Xaml;
using System.Collections.ObjectModel;

namespace MiniTC.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy PanelTC.xaml
    /// </summary>
    public partial class PanelTC : UserControl, IPanelTC
    {
        protected static readonly DependencyProperty CurrentPathProperty = DependencyProperty.Register(nameof(CurrentPath), typeof(string), typeof(PanelTC), new PropertyMetadata(null));
        protected static readonly DependencyProperty AvailableDrivesProperty = DependencyProperty.Register(nameof(AvailableDrives), typeof(ObservableCollection<string>), typeof(PanelTC), new PropertyMetadata(null));
        protected static readonly DependencyProperty CurrentPathContentProperty = DependencyProperty.Register(nameof(CurrentPathContent), typeof(ObservableCollection<string>), typeof(PanelTC), new PropertyMetadata(null));
        protected static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register(nameof(SelectedPath), typeof(string), typeof(PanelTC), new PropertyMetadata(null));
        protected static readonly DependencyProperty SelectedDriveProperty = DependencyProperty.Register(nameof(SelectedDrive), typeof(string), typeof(PanelTC), new PropertyMetadata(null));

        public string CurrentPath 
        { 
            get { return (string)GetValue(CurrentPathProperty); } 
            set { SetValue(CurrentPathProperty, value); }
        }
        public ObservableCollection<string> AvailableDrives 
        {
            get { return (ObservableCollection<string>)GetValue(AvailableDrivesProperty); }
            set { SetValue(AvailableDrivesProperty, value); } 
        }
        public ObservableCollection<string> CurrentPathContent 
        {
            get { return (ObservableCollection<string>)GetValue(CurrentPathContentProperty); }
            set { SetValue(CurrentPathContentProperty, value); }
        }
        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }
        public string SelectedDrive 
        {
            get { return (string)GetValue(SelectedDriveProperty); }
            set { SetValue(SelectedDriveProperty,value); }
        }

        public PanelTC()
        {
            InitializeComponent();
        }
    }
}

using LibVLCSharp.Shared;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Owin.Hosting;
using LibVLCSharp.Forms.Shared;

namespace Serwer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string dbPath = @".\VideoSources.db";
        public static LibVLC _libvlc;
        public LibVLCSharp.Shared.MediaPlayer outputMediaPlayer;
        public Media outputmedia;
        public IDisposable SignalR { get; set; }
        public MainWindow()
        {
            InitializeComponent();            // this will load the native libvlc library (if needed, depending on the platform). 
            Core.Initialize();                // instantiate the main libvlc object
            if (!System.IO.File.Exists(dbPath))
            {
                using (var db = new SQLiteConnection(dbPath))
                {
                    db.CreateTable<VideoSources>();
                    db.Close();
                }
            }

            _libvlc = new LibVLC(new[]
            {
                "--verbose=2",
                "--file-logging",
                "--logfile=D:\\vlc-log.txt"
            });
            outputMediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc);
            VideoView.MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libvlc) { Mute = true };
            Combobox.ItemsSource = SettingsWindow.GetDatabaseList();
        }
        private void OnClickStartSrv(object sender, RoutedEventArgs e)
        {
            string url = @"https://" +  Properties.Settings.Default.LocalAddress + ":8080";
            SignalR = WebApp.Start<Startup> (url);
            StartItem.IsEnabled = false;
            StopItem.IsEnabled = true;
        }
        private void OnClickStopSrv(object sender, RoutedEventArgs e)
        {
            SignalR.Dispose();
            StartItem.IsEnabled = true;
            StopItem.IsEnabled = false;
        }
        private void OnClickSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }

        private void Combobox_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            VideoSources selecteditem = Combobox.SelectedItem as VideoSources;
            VideoView.MediaPlayer.Stop();
            VideoView.MediaPlayer.Media = new Media
                (_libvlc, selecteditem.Address, FromType.FromLocation);
            VideoView.MediaPlayer.Media.AddOption(":no-audio");
            VideoView.MediaPlayer.Play();
        }
    }
}

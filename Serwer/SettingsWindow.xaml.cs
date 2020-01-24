using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Serwer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            lbVideoSourcesList.ItemsSource = GetDatabaseList();
        }
        public static List<VideoSources> GetDatabaseList()     
        {
            var db = new SQLiteConnection(MainWindow.dbPath);
            List<VideoSources> videosList = db.Table<VideoSources>().ToList();
            db.Close();
            return videosList;
        }
        private void DeleteFromTable(long id)
        {
            var db = new SQLiteConnection(MainWindow.dbPath);
            db.Delete<VideoSources>(id);
            db.Close();
        }
        private void AddToTable(VideoSources newSrc)        
        {
            var db = new SQLiteConnection(MainWindow.dbPath);
            db.Insert(newSrc);
            db.Close();
        }
        private void BtnAddNewAddress_Click(object sender, RoutedEventArgs e) 
        {
            AddAddressDialog addressDialog = new AddAddressDialog();
            if(addressDialog.ShowDialog()==true)
            {
                AddToTable(addressDialog.NewVideoSource);
                lbVideoSourcesList.ItemsSource = GetDatabaseList();
            }
        }
        private void BtnDeleteSelectedAddress_Click(object sender, RoutedEventArgs e)       //Metoda usuwająca zaznaczone wpisy z bazy
        {
            foreach (VideoSources o in lbVideoSourcesList.SelectedItems)
            {
                DeleteFromTable(o.Id);
            }
            lbVideoSourcesList.ItemsSource = GetDatabaseList();
        }


        private void Settings_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            var mainwindow = (MainWindow)Application.Current.MainWindow;
            mainwindow.Combobox.ItemsSource = GetDatabaseList();
        }

    }
}

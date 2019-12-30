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
            UpdateDatabaseList();
        }
        private void UpdateDatabaseList()     //Metoda synchronizująca listbox z bazą danych
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            List<VideoSources> videosList = db.Table<VideoSources>().ToList();
            db.Close();
            lbVideoSourcesList.ItemsSource = videosList;
        }
        private void DeleteFromTable(long id)            //Metoda usuwająca wpis z bazy danych w oparciu o ID wpisu
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            db.Delete<VideoSources>(id);
            db.Close();
        }
        private void AddToTable(VideoSources newSrc)        //Metoda dodająca wpis do bazy danych
        {
            var databasePath = "D:\\Projects\\Sowa\\Sowa\\Serwer\\VideoSources.db";
            var db = new SQLiteConnection(databasePath);
            db.Insert(newSrc);
            db.Close();
        }
        private void BtnAddNewAddress_Click(object sender, RoutedEventArgs e)   //Wywołanie okna dodawania nowego wpisu
        {
            AddAddressDialog addressDialog = new AddAddressDialog();
            if(addressDialog.ShowDialog()==true)
            {
                AddToTable(addressDialog.NewVideoSource);
                UpdateDatabaseList();
            }
        }
        private void BtnDeleteSelectedAddress_Click(object sender, RoutedEventArgs e)       //Metoda usuwająca zaznaczone wpisy z bazy
        {
            foreach (VideoSources o in lbVideoSourcesList.SelectedItems)
            {
                DeleteFromTable(o.Id);
            }
            UpdateDatabaseList();
        }


        private void Settings_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

    }
}

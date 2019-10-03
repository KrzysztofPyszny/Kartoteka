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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;       //Umożliwia tworzenie obiektów MySql

namespace Kartoteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection conn;       //Obiekt połączenia z bazą MySql

        public static int blokuj = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            //Zczytywanie data z pliku
            DateTime dataTeraz = DateTime.Now;       //pobieram obecną datę w celu porównania z tą w pliku
            string dataWpliku;      //kontener na datę z dokumentu
            System.IO.StreamReader dataTXT = new System.IO.StreamReader("DataKopii.txt");      //Wskazanie pliku dla StreamReadera

            MySqlConnectionStringBuilder connString = new MySqlConnectionStringBuilder();       //Obiekt tworzący łańcuch połączeniowy z bazą
            connString.Server = "DESKTOP-OBR3EBS"/*"LAPTOP-EEGICNKI"*/;      //Przekazanie nazwy serwera,
            connString.Database = "KartotekaAdamPosluszny";     //nazwy bazy danych,
            string login = txtLogin.Text;
            if (login.EndsWith("1"))
            {
                string[] loginStr = login.Split(' ');
                connString.UserID = loginStr[0];
                blokuj = 1;
            }
            else
            {
                connString.UserID = login;
            }
            connString.Password = pswHaslo.Password;        //oraz hasła do łańcucha połączeniowego

            conn = new MySqlConnection(connString.ConnectionString);        //Przekazanie łańcucha do obiektu conn

            try         //Próba nawiązania połączenia z bazą
            {
                dataWpliku = dataTXT.ReadLine();        //odczytanie 1 linii z pliku DataKopii.txt
                dataTXT.Close();        //zamknięcie pliku
                DateTime dataZpliku = Convert.ToDateTime(dataWpliku);        //konwertuję datę z pliku na typ DateTime w celu porównania
                TimeSpan roznicaDat = dataTeraz - dataZpliku;       //tworze TimeSpana żeby zapisać różnicę między datami
                int dni = roznicaDat.Days;      //wyciągam całkowitą liczbę dni pomiędzy Datami

                conn.Open();        //Otwarcie połączenia z bazą

                if (dni > 30)        //jeżeli od ostatniego backupu minęło więcej niż 30 dni czyli miesiąc
                {
                    //Tu będzie kod robiący eksport bazy
                    string lokalizacjaKopii = /*NIE ZAPOMNIJ ZMIENIĆ ŚCIEŻKI*/ @"C:\Kartoteka\KopieBazy\" + dataTeraz.ToShortDateString() + "kartoteka.sql";   //ścieżka do pliku po eksporcie
                    using (MySqlCommand polecenie = new MySqlCommand())
                    {
                        using (MySqlBackup backup = new MySqlBackup(polecenie))     //Był potrzebny pakiet MySqlBackup.Net
                        {
                            polecenie.Connection = conn;
                            backup.ExportToFile(lokalizacjaKopii);
                        }
                    }
                    //i nadpisujący plik DataKopii.txt datą utworzenia
                    System.IO.StreamWriter dataTXTnadpisz = new System.IO.StreamWriter("DataKopii.txt", false);     //StreamWriter z parametrem false nadpisuje cały plik
                    dataTXTnadpisz.Write(dataTeraz.ToString());     //Zapisuję nową datę w pliku
                    dataTXTnadpisz.Close();     //zamykam plik
                    MessageBox.Show("Wykonano kopię bazy danych!");
                }


                PacjenciOsobowe pacjenciOsobowe = new PacjenciOsobowe(conn);        //Utworzenie nowe obiektu klasy PacjenciOsobowe i przekazanie do niego połączenia conn
                pacjenciOsobowe.Show();     //Wywołanie metody Show dla obiektu pacjenciOsobowe
                this.Close();       //Zamknięcie okna logowania metodą Close
            }
            catch(MySqlException ex)        //Przechwycenie błędu połączenia z bazą
            {
                MessageBox.Show(ex.Message);        //Wyświetlenie treści błędu
            }
        }
    }
}

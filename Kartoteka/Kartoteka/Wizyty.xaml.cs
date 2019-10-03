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
using MySql.Data.MySqlClient;       //Umożliwia tworzenie obiektów MySql
using System.Data;      //Umożliwia korzystanie z modelu bezpołączeniowego (DataSet, DataTable)

namespace Kartoteka
{
    /// <summary>
    /// Interaction logic for Wizyty.xaml
    /// </summary>
    public partial class Wizyty : Window
    {
        MySqlConnection conn;       //Obiekt połączenia z bazą MySql
        string id;      //Zmienna przechowująca id pacjenta
        MySqlCommand query;     //Obiekt zapytania do bazy
        DataSet ds;     //Obiekt DataSet do przechowania danych z bazy
        MySqlDataAdapter dataAdapter;       //Obiekt, który przekaże dane z bazy do DataSet
        DataRowView row;        //Obiekt wiersza DataGrid 
        int i;

        public Wizyty()
        {
            InitializeComponent();
        }

        public Wizyty(MySqlConnection conn, string id)     //Przeciążenie metody PacjenciOsobowe o obiekt połączenia conn
        {
            InitializeComponent();

            this.conn = conn;       //Przekazanie połączenia conn
            this.id = id;       //Przekazanie id pacjenta


            wypelnijDg(conn, id);
        }

        private void wypelnijDg(MySqlConnection conn, string id)
        {
            query = new MySqlCommand();     //Nowy obiekt klasy MySqlCommand 
            query.Connection = conn;        //Przypisanie połączenia
            query.CommandText = "select * from Wizyty where Id_pacjenta = @IdPacjenta";        //oraz treści sparametryzwanego zapytania z klauzula where do obiektu query
            query.Parameters.AddWithValue("@IdPacjenta", id);       //Przypisanie wartości id do parametru

            ds = new DataSet();     //Utworzenie DataSet
            dataAdapter = new MySqlDataAdapter(query);      //Utworzenie dataAdapter i przekazanie do niego zapytania query
            dataAdapter.Fill(ds, "Wizyty");        //Wypełnienie tabeli Wizyty w ds danymi przy użyciu metody Fill

            dgWizyty.ItemsSource = ds.Tables["Wizyty"].DefaultView;       //Wskazanie jako źródło danych ItemSource doyślny widok DataSet ds tabeli Wizyty

            

            if (ds.Tables["Wizyty"].Rows.Count != 0)
            {
                dgWizyty.ScrollIntoView(dgWizyty.Items[dgWizyty.Items.Count - 1]);       //Ustawienie widoku DataGrid na samym dole listy
                dgWizyty.UpdateLayout();       //Aktualizacja przewinięcia widoku na dół listy
                dgWizyty.SelectedIndex = dgWizyty.Items.Count - 1;
            }
        }

        private void DgWizyty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;     //Powiązanie DataGrid z nowym obiektem dg
            row = dg.SelectedItem as DataRowView;       //Przypisanie aktywnego wiersza SelectedItem do nowego obiektu row            

            if (row != null)     //Sprawdzam czy wybrano jakikolwiek wiersz
            {
                //BLOKOWANIE WIZYT-----------------------------
                if(MainWindow.blokuj == 1)
                {
                    dgWizyty.CanUserDeleteRows = false;
                    if (row["Zmiany"].ToString() != "")
                    {
                        txtZmiany.IsReadOnly = true;
                    }
                    else
                    {
                        txtZmiany.IsReadOnly = false;
                    }
                    if (row["Zalecenia"].ToString() != "")
                    {
                        txtZalecenia.IsReadOnly = true;
                    }
                    else
                    {
                        txtZalecenia.IsReadOnly = false;
                    }
                    if (row["Zabiegi"].ToString() != "")
                    {
                        txtZabiegi.IsReadOnly = true;
                    }
                    else
                    {
                        txtZabiegi.IsReadOnly = false;
                    }
                }
                

                dpDataWizyty.Text = row["Data_wizyty"].ToString();      //Jeśli tak to wiążę dane z wybranego wiersza z formularzem
                txtZmiany.Text = row["Zmiany"].ToString();
                txtZalecenia.Text = row["Zalecenia"].ToString();
                txtZabiegi.Text = row["Zabiegi"].ToString();        //Dodaje zabiegi
                txtNotatki.Text = row["Notatki"].ToString();        //Dodaje notatki
            }
        }
        
        private void DpDataWizyty_LostFocus(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Data_wizyty"] = Convert.ToDateTime(dpDataWizyty.SelectedDate.Value.ToShortDateString());
            }
        }

        private void TxtZmiany_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zmiany"] = txtZmiany.Text;
            }
        }

        private void TxtZalecenia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zalecenia"] = txtZalecenia.Text;
            }
        }

        private void TxtZabiegi_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zabiegi"] = txtZabiegi.Text;
            }
        }

        private void TxtNotatki_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Notatki"] = txtNotatki.Text;
            }
        }

        private void Zapisz()
        {
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);      //Nowy obiekt klasy CommandBuilder, który ułoży sparametryzowane zapytanie (update,insert,delet) do bazy z przekazanym dataAdapter

            try         //Próba wykonania aktualizacji danych
            {
                i = dgWizyty.SelectedIndex;
                dataAdapter.Update(ds, "Wizyty");      //przy użyciu metody Update klasy DataAdapter na ds tabeli Wizyty

                ds.Tables["Wizyty"].Clear();       //Wyczyszczenie tabeli ze starych danych
                dataAdapter.Fill(ds, "Wizyty");        //i ponowne jej wypełnienie

                dgWizyty.SelectedIndex = i;

                if (ds.Tables["Wizyty"].Rows.Count != 0)
                {
                    dgWizyty.ScrollIntoView(dgWizyty.Items[dgWizyty.Items.Count - 1]);       //Ustawienie widoku DataGrid na samym dole listy
                    dgWizyty.UpdateLayout();       //Aktualizacja przewinięcia widoku na dół listy
                }
            }
            catch (MySqlException ex)        //Przechwycenie błędu nieudanej próby aktualizacji
            {
                MessageBox.Show(ex.Message);        //Wyświetlenie treści błędu
            }
        }

        private void BtnZapisz_Click(object sender, RoutedEventArgs e)
        {
            Zapisz();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string tresc = "Zapisać zmiany przed wyjściem?";
            string tytul = "Zamykanie";
            MessageBoxButton przyciski = MessageBoxButton.YesNoCancel;
            MessageBoxResult wynik = MessageBox.Show(tresc, tytul, przyciski);

            switch (wynik)
            {
                case MessageBoxResult.Yes:
                    Kartoteka.PacjenciOsobowe.licznikOkien--;
                    if (Kartoteka.PacjenciOsobowe.licznikOkien == 0)
                    {
                        Kartoteka.PacjenciOsobowe.AppWindow.btnZapisz.IsEnabled = true;
                        Zapisz();
                    }
                    else
                    {
                        Zapisz();
                    }
                    break;
                case MessageBoxResult.No:
                    Kartoteka.PacjenciOsobowe.licznikOkien--;
                    if (Kartoteka.PacjenciOsobowe.licznikOkien == 0)
                    {
                        Kartoteka.PacjenciOsobowe.AppWindow.btnZapisz.IsEnabled = true;
                    }
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void BtnDodajW_Click(object sender, RoutedEventArgs e)
        {
            if(row != null)
            {
                Zapisz();
            }

            string data = DateTime.Now.ToString("yyyy-MM-dd");
                
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandText = "insert into Wizyty (Data_wizyty,Id_pacjenta) values(@data,@id);";
            command.Parameters.AddWithValue("@data", data);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            wypelnijDg(conn, id);
        }
    }
}

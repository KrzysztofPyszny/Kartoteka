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
using System.ComponentModel;

namespace Kartoteka
{
    /// <summary>
    /// Interaction logic for PacjenciOsobowe.xaml
    /// </summary>

    public partial class PacjenciOsobowe : Window
    {
        MySqlConnection conn;       //Obiekt połączenia z bazą MySql
        MySqlCommand query;     //Obiekt zapytania do bazy
        DataSet ds;     //Obiekt DataSet do przechowania danych z bazy
        MySqlDataAdapter dataAdapter;       //Obiekt, który przekaże dane z bazy do DataSet
        DataRowView row;        //Obiekt wiersza DataGrid 
        string wybor;       //Przechowuje wybraną wartość z cbWybor
        string id;      //Zmienna przechowująca id pacjenta  

        public PacjenciOsobowe()
        {
            InitializeComponent();
        }

        public static PacjenciOsobowe AppWindow;

        public static int licznikOkien = 0;

        public PacjenciOsobowe(MySqlConnection conn)        //Przeciążenie metody PacjenciOsobowe o obiekt połączenia conn
        {
            InitializeComponent();

            this.conn = conn;       //Przekazanie połączenia conn
            AppWindow = this;

            wypelnijDg(conn);

            if (dgPacjenciOsobowe.HasItems)
            {
                dgPacjenciOsobowe.SelectedItem = dgPacjenciOsobowe.Items[0];
            }
            
        }

        private void wypelnijDg(MySqlConnection conn)
        {
            query = new MySqlCommand();     //Nowy obiekt klasy MySqlCommand 
            query.Connection = conn;        //Przypisanie połączenia
            query.CommandText = "select * from PacjenciOsobowe order by Nazwisko";        //oraz treści zapytania do obiektu query

            ds = new DataSet();     //Utworzenie DataSet
            dataAdapter = new MySqlDataAdapter(query);      //Utworzenie dataAdapter i przekazanie do niego zapytania query
            dataAdapter.Fill(ds, "PacjenciOsobowe");        //Wypełnienie tabeli PacjenciOsobowe w ds danymi przy użyciu metody Fill
            dgPacjenciOsobowe.ItemsSource = ds.Tables["PacjenciOsobowe"].DefaultView;       //Wskazanie jako źródło danych ItemSource doyślny widok DataSet ds tabeli PacjenciOsobowe
            dgPacjenciOsobowe.Items.SortDescriptions.Add(new SortDescription("Nazwisko", ListSortDirection.Ascending));     //nowe sortowanie (naprawia Ł)
            dgPacjenciOsobowe.Items.SortDescriptions.Add(new SortDescription("Imie", ListSortDirection.Ascending));     //nowe sortowanie (naprawia Ł)
            dgPacjenciOsobowe.Items.SortDescriptions.Add(new SortDescription("Id_pacjenta", ListSortDirection.Ascending));     //nowe sortowanie (naprawia Ł)
        }

        private void DgPacjenciOsobowe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;     //Powiązanie DataGrid z nowym obiektem dg
            row = dg.SelectedItem as DataRowView;       //Przypisanie aktywnego wiersza SelectedItem do nowego obiektu row

            rbMezczyzna.IsChecked = false;
            rbKobieta.IsChecked = false;

            if (row != null)     //Sprawdzam czy wybrano jakikolwiek wiersz
            {
                if (row["Plec"].ToString() == "M")
                {
                    rbMezczyzna.IsChecked = true;
                }
                if (row["Plec"].ToString() == "K")
                {
                    rbKobieta.IsChecked = true;
                }
                id = row["Id_pacjenta"].ToString();     //Przypisanie id do string
                txtImie.Text = row["Imie"].ToString();      //Jeśli tak to wiążę dane z wybranego wiersza z formularzem
                txtNazwisko.Text = row["Nazwisko"].ToString();
                if(row["Data_urodzenia"].ToString() == "")
                {
                    dpDataUrodzenia.Text = "01.01.2000";
                }
                else
                {
                    dpDataUrodzenia.Text = row["Data_urodzenia"].ToString();
                }
                txtDiagnozaMedyczna.Text = row["Diagnoza_medyczna"].ToString();
                txtRodzice.Text = row["Rodzice"].ToString();
                txtAdres.Text = row["Adres"].ToString();
                txtMiejscowosc.Text = row["Miejscowosc"].ToString();
                txtKodPocztowy.Text = row["Kod_pocztowy"].ToString();
                txtTelefon.Text = row["Telefon"].ToString();
                txtEmail.Text = row["Email"].ToString();
                txtPESEL.Text = row["PESEL"].ToString();
            }
        }

        private void Zapisz()
        {
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);      //Nowy obiekt klasy CommandBuilder, który ułoży sparametryzowane zapytanie (update,insert,delet) do bazy z przekazanym dataAdapter

            int index;
            if (row != null){
                index = dgPacjenciOsobowe.SelectedIndex;        //pobieram numer wiersza przed  resetem datagrida
            }
            else
            {
                index = 0;
            }

            try         //Próba wykonania aktualizacji danych
            {
                dataAdapter.Update(ds, "PacjenciOsobowe");      //przy użyciu metody Update klasy DataAdapter na ds tabeli PacjenciOsobowe

                ds.Tables["PacjenciOsobowe"].Clear();       //Wyczyszczenie tabeli ze starych danych
                wypelnijDg(conn);

                if (dgPacjenciOsobowe.HasItems)
                {
                    dgPacjenciOsobowe.SelectedItem = dgPacjenciOsobowe.Items[index];        //i wybieram go ponownie
                    dgPacjenciOsobowe.ScrollIntoView(dgPacjenciOsobowe.Items[index]);       //Ustawienie widoku DataGrid na samym dole listy
                    dgPacjenciOsobowe.UpdateLayout();       //Aktualizacja przewinięcia widoku na dół listy
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
            if(licznikOkien != 0)
            {
                MessageBox.Show("Nie zamknąłeś wszystkich okien!");
                e.Cancel = true;
            }
            else
            {
                string tresc = "Zapisać zmiany przed wyjściem?";
                string tytul = "Zamykanie";
                MessageBoxButton przyciski = MessageBoxButton.YesNoCancel;
                MessageBoxResult wynik = MessageBox.Show(tresc, tytul, przyciski);

                switch (wynik)
                {
                    case MessageBoxResult.Yes:
                        Zapisz();
                        conn.Close();
                        break;
                    case MessageBoxResult.No:
                        conn.Close();
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }


        //Poniżej wiążę poszczególne pola z odpowiadającymi im kolumnami w wybranym wierszu

        private void TxtImie_TextChanged(object sender, TextChangedEventArgs e)     //Zmiany doknonywane są przy zmianie pola tekstowego TextChange
        {
            if (row == null)        //Zabezpieczenie przed póstym polem 
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Imie"] = txtImie.Text;     //Wiązanie z DataGrid
            }
        }

        private void TxtNazwisko_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Nazwisko"] = txtNazwisko.Text;
            }
        }

        private void DpDataUrodzenia_LostFocus(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Data_urodzenia"] = Convert.ToDateTime(dpDataUrodzenia.SelectedDate.Value.ToShortDateString());
            }
        }

        private void TxtDiagnozaMedyczna_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Diagnoza_medyczna"] = txtDiagnozaMedyczna.Text;
            }
        }

        private void TxtRodzice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Rodzice"] = txtRodzice.Text;
            }
        }

        private void TxtAdres_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Adres"] = txtAdres.Text;
            }
        }

        private void TxtMiejscowosc_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Miejscowosc"] = txtMiejscowosc.Text;
            }
        }

        private void TxtKodPocztowy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Kod_pocztowy"] = txtKodPocztowy.Text;
            }
        }

        private void TxtTelefon_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Telefon"] = txtTelefon.Text;
            }
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Email"] = txtEmail.Text;
            }
        }

        private void CbWybor_SelectionChanged(object sender, SelectionChangedEventArgs e)       //Wybór kategorii wyszukiwania
        {
            wybor = ((ComboBoxItem)cbWybor.SelectedValue).Content.ToString();       //przypisuje wybraną wartość do stringa w celu późniejszego wykorzystania w zapytaniu
            if(wybor == "Wszystko")
            {
                string stareWyszukanie = txtWyszukaj.Text;
                txtWyszukaj.Clear();
                txtWyszukaj.Text = stareWyszukanie;
            }
        }

        private void TxtWyszukaj_TextChanged(object sender, TextChangedEventArgs e)     //Automatyczne wyszukiwanie w chwili wprowadzenia danych do TextBox
        {
            query.Parameters.Clear();       //Czyszczenie parametrów query w celu uniknięcia Exception
            switch (wybor)      //pętla switch zmieniająca wykonany fragment kodu w zależności o wcześniej zapisanej wartości w stringu
            {
                case "Wszystko":        //Jeśli wybrano wszystko
                    ds.Tables["PacjenciOsobowe"].Clear();       //Czyszczenie tablicy DataSet ds PacjenciOsobowe
                    query.CommandText = "select * from PacjenciOsobowe order by Nazwisko";        //Nowa treść zapytania o wszystkich pacjentów
                    dataAdapter = new MySqlDataAdapter(query);      //przekazanie do już istniejącego adaptera
                    dataAdapter.Fill(ds, "PacjenciOsobowe");        //wypełnienie DataSet
                    break;      //koniec przypadku case
                case "Imię":
                    ds.Tables["PacjenciOsobowe"].Clear();
                    query.CommandText = "select * from PacjenciOsobowe where Imie like @Imie";      //Nowe sparametryzowane zapytanie z klauzulą where która ogranicza wynik o konkretne imiona
                    query.Parameters.AddWithValue("@Imie", txtWyszukaj.Text + "%");     //Przypisanie wartości parametru (wpisana wartość + dowolny ciąg znaków[%]) do zapytania query
                    dataAdapter = new MySqlDataAdapter(query);
                    dataAdapter.Fill(ds, "PacjenciOsobowe");
                    break;
                case "Nazwisko":
                    ds.Tables["PacjenciOsobowe"].Clear();
                    query.CommandText = "select * from PacjenciOsobowe where Nazwisko like @Nazwisko";
                    query.Parameters.AddWithValue("@Nazwisko", txtWyszukaj.Text + "%");
                    dataAdapter = new MySqlDataAdapter(query);
                    dataAdapter.Fill(ds, "PacjenciOsobowe");
                    break;
                default:
                    MessageBox.Show("Wystąpił nieoczekiwany błąd!");        //Zabezpieczenie nieoczekiwanego błędu
                    break;
            }
        }

        private void BtnWizyty_Click(object sender, RoutedEventArgs e)      //Akcja wykona się po naciśnięciu przycisku Wizyty
        {            
            if(row == null || row["Id_pacjenta"].ToString() == "")      //Zabezpieczenie przed nie wybraniem pacjenta
            {
                MessageBox.Show("Upewnij się, że wybrałeś zapisanego pacjenta!");
            }
            else
            {
                btnZapisz.IsEnabled = false;
                licznikOkien++;
                Wizyty wizyty = new Wizyty(conn, id);        //Przekazanie połączenia, id do obiektu wizyty
                wizyty.Show();      //Wyświetlenie okna wizyty używając metody Show
            }
        }

        private void BtnZdjFil_Click(object sender, RoutedEventArgs e)
        {
            if (row == null || row["Id_pacjenta"].ToString() == "")      //Zabezpieczenie przed nie wybraniem pacjenta
            {
                MessageBox.Show("Upewnij się, że wybrałeś zapisanego pacjenta!");
            }
            else
            {
                ZdjeciaFilmy.UtworzWyswietl(row["Id_pacjenta"].ToString(), row["Nazwisko"].ToString(), row["Imie"].ToString());
            }
        }


        private void BtnMedyczne_Click(object sender, RoutedEventArgs e)
        {
            if (row == null || row["Id_pacjenta"].ToString() == "")      //Zabezpieczenie przed nie wybraniem pacjenta
            {
                MessageBox.Show("Upewnij się, że wybrałeś zapisanego pacjenta!");
            }
            else
            {
                btnZapisz.IsEnabled = false;
                licznikOkien++;
                PacjenciMedyczne pacjenciMedyczne = new PacjenciMedyczne(conn, id);
                pacjenciMedyczne.Show();
            }
        }

        private void BtnPdfEksport_Click(object sender, RoutedEventArgs e)
        {
            if(row == null || row["Id_pacjenta"].ToString() == "")
            {
                MessageBox.Show("Upewnij się, że wybrałeś zapisanego pacjenta!");
            }
            else
            {
                //Dane z Tabeli PacjenciOsobowe zapisane do DataSeta
                MySqlCommand commandPDF = new MySqlCommand();
                commandPDF.Connection = conn;
                commandPDF.CommandText = "select * from PacjenciOsobowe where id_pacjenta = @idPacjenta";
                commandPDF.Parameters.AddWithValue("@idPacjenta", row["Id_pacjenta"].ToString());

                DataSet dsPDF = new DataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter(commandPDF);
                adapter.Fill(dsPDF, "PacjenciOsobowe");

                //Dane z Tabeli PacjenciMedyczne zapisane do DataSeta
                MySqlCommand commandPDFmedyczne = new MySqlCommand();
                commandPDFmedyczne.Connection = conn;
                commandPDFmedyczne.CommandText = "select * from PacjenciMedyczne where id_pacjenta = @idPacjenta";
                commandPDFmedyczne.Parameters.AddWithValue("@idPacjenta", row["Id_pacjenta"].ToString());

                MySqlDataAdapter adapterMedyczne = new MySqlDataAdapter(commandPDFmedyczne);
                adapterMedyczne.Fill(dsPDF, "PacjenciMedyczne");

                //Dane z Tabeli Wizyty zapisane do DataSeta
                MySqlCommand commandPDFwizyty = new MySqlCommand();
                commandPDFwizyty.Connection = conn;
                commandPDFwizyty.CommandText = "select * from Wizyty where id_pacjenta = @idPacjenta";
                commandPDFwizyty.Parameters.AddWithValue("@idPacjenta", row["Id_pacjenta"].ToString());

                MySqlDataAdapter adapterWizyty = new MySqlDataAdapter(commandPDFwizyty);
                adapterWizyty.Fill(dsPDF, "Wizyty");

                if(dsPDF.Tables["PacjenciMedyczne"].Rows.Count == 0)    //Zabezpieczenie przed pacjentem który ma tylko dane osobowe
                {
                    dsPDF.Tables["PacjenciMedyczne"].Rows.Add();
                }

                //Przekazanie

                try
                {
                    string nazwaPliku = dsPDF.Tables["PacjenciOsobowe"].Rows[0][2].ToString() + dsPDF.Tables["PacjenciOsobowe"].Rows[0][1].ToString() + dsPDF.Tables["PacjenciOsobowe"].Rows[0][0].ToString();
                    string tytulPliku = dsPDF.Tables["PacjenciOsobowe"].Rows[0][1].ToString() + " " + dsPDF.Tables["PacjenciOsobowe"].Rows[0][2].ToString();

                    ExportPDF.ExportDataSetToPdf(dsPDF, @"C:\Kartoteka\PacjenciRaporty\" + nazwaPliku + ".pdf", "Raport " + tytulPliku);
                    System.Diagnostics.Process.Start(@"C:\Kartoteka\PacjenciRaporty\" + nazwaPliku + ".pdf");
                }
                catch
                {
                    MessageBox.Show("Upewnij się, że wybrałeś zapisanego pacjenta!");
                }
                
            }
        }
                

        private void BtnDodajP_Click(object sender, RoutedEventArgs e)
        {
            if (row != null)
            {
                Zapisz();
            }

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandText = "insert into PacjenciOsobowe values();";
            command.ExecuteNonQuery();
            wypelnijDg(conn);

            if (ds.Tables["PacjenciOsobowe"].Rows.Count != 0)
            {
                dgPacjenciOsobowe.SelectedIndex = 0;
                dgPacjenciOsobowe.ScrollIntoView(dgPacjenciOsobowe.Items[0]);       //Ustawienie widoku DataGrid na samym dole listy
                dgPacjenciOsobowe.UpdateLayout();       //Aktualizacja przewinięcia widoku na dół listy
            }
        }

        private void RbMezczyzna_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Plec"] = "M";
            }
        }

        private void RbKobieta_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Plec"] = "K";
            }
        }

        private void TxtPESEL_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["PESEL"] = txtPESEL.Text;
            }
        }

        private void TxtWyszukaj_GotFocus(object sender, RoutedEventArgs e)
        {
            string stareWyszukanie = txtWyszukaj.Text;
            txtWyszukaj.Clear();
            txtWyszukaj.Text = stareWyszukanie;
        }
    }
}

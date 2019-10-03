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
    /// Interaction logic for PacjenciMedyczne.xaml
    /// </summary>
    public partial class PacjenciMedyczne : Window
    {
        MySqlConnection conn;       //Obiekt połączenia z bazą MySql
        string id;      //Zmienna przechowująca id pacjenta
        MySqlCommand query;     //Obiekt zapytania do bazy
        DataSet ds;     //Obiekt DataSet do przechowania danych z bazy
        MySqlDataAdapter dataAdapter;       //Obiekt, który przekaże dane z bazy do DataSet
        DataRowView row;        //Obiekt wiersza DataGrid 
        int i;

        public PacjenciMedyczne()
        {
            InitializeComponent();
        }

        public PacjenciMedyczne(MySqlConnection conn, string id)     //Przeciążenie metody PacjenciMedyczne o obiekt połączenia conn
        {
            InitializeComponent();

            this.conn = conn;       //Przekazanie połączenia conn
            this.id = id;       //Przekazanie id pacjenta

            wypelnijDg(conn, id);

            if (ds.Tables["PacjenciMedyczne"].Rows.Count == 0)      //Jeżeli nic nie wcyztało się do tabeli pacjenici medyczne
            {
                dodajDaneMedyczne();        //uruchamiam metodę ktora ma dodac jeden wiersz
            }
        }

        private void dodajDaneMedyczne()            //Zapewnia istnienie przynajmniej jednej linijki w tabeli pacjecnciMedyczne
        {
            MySqlCommand command = new MySqlCommand();
            command.Connection = conn;
            command.CommandText = "insert into PacjenciMedyczne (Id_pacjenta) values(@id);";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            wypelnijDg(conn, id);
        }

        private void wypelnijDg(MySqlConnection conn, string id)
        {
            query = new MySqlCommand();     //Nowy obiekt klasy MySqlCommand 
            query.Connection = conn;        //Przypisanie połączenia
            query.CommandText = "select * from PacjenciMedyczne where Id_pacjenta = @IdPacjenta";        //oraz treści sparametryzwanego zapytania z klauzula where do obiektu query
            query.Parameters.AddWithValue("@IdPacjenta", id);       //Przypisanie wartości id do parametru

            ds = new DataSet();     //Utworzenie DataSet
            dataAdapter = new MySqlDataAdapter(query);      //Utworzenie dataAdapter i przekazanie do niego zapytania query
            dataAdapter.Fill(ds, "PacjenciMedyczne");        //Wypełnienie tabeli PacjenciMedyczne w ds danymi przy użyciu metody Fill

            dgDaneMedyczne.ItemsSource = ds.Tables["PacjenciMedyczne"].DefaultView;       //Wskazanie jako źródło danych ItemSource doyślny widok DataSet ds tabeli PacjenciMedyczne

            if (ds.Tables["PacjenciMedyczne"].Rows.Count != 0)
            {
                dgDaneMedyczne.SelectedIndex = dgDaneMedyczne.Items.Count - 1;
            }
        }

        private void DgDaneMedyczne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;     //Powiązanie DataGrid z nowym obiektem dg
            row = dg.SelectedItem as DataRowView;       //Przypisanie aktywnego wiersza SelectedItem do nowego obiektu row

            rbWterminie.IsChecked = false;
            rbPozaTerminem.IsChecked = false;
            rbSilNatTak.IsChecked = false;
            rbSilNatNie.IsChecked = false;
            rbKoniecznosc.IsChecked = false;
            rbPlanowane.IsChecked = false;
            rbProbaTrakcyjnaTak.IsChecked = false;
            rbProbaTrakcyjnaNie.IsChecked = false;
            rbProbTrakSymTak.IsChecked = false;
            rbProbTrakSymNie.IsChecked = false;
            rbZawProTak.IsChecked = false;
            rbZawProNie.IsChecked = false;
            rbZawPronSymTak.IsChecked = false;
            rbZawPronSymNie.IsChecked = false;
            rbZawBoczLTak.IsChecked = false;
            rbZawBoczLNie.IsChecked = false;
            rbZawBoczPTak.IsChecked = false;
            rbZawBoczPNie.IsChecked = false;
            rbZawBoczAsL.IsChecked = false;
            rbZawBoczAsP.IsChecked = false;

            if (row != null)     //Sprawdzam czy wybrano jakikolwiek wiersz
            {
                txtProWtrakCia.Text = row["Problemy_ciaza"].ToString();

                if (row["Porod_w_terminie"].ToString() == "T")
                {
                    rbWterminie.IsChecked = true;
                }

                if (row["Porod_poza_terminem"].ToString() == "T")
                {
                    rbPozaTerminem.IsChecked = true;
                }

                if (row["Porod_silami_natury"].ToString() == "T")
                {
                    rbSilNatTak.IsChecked = true;
                }

                if (row["Porod_silami_natury"].ToString() == "N")
                {
                    rbSilNatNie.IsChecked = true;
                }

                if (row["Cesarskie_ciecie_planowane"].ToString() == "T")
                {
                    rbPlanowane.IsChecked = true;
                }

                if (row["Cesarskie_ciecie_koniecznosc"].ToString() == "T")
                {
                    rbKoniecznosc.IsChecked = true;
                }
                if (row["Proba_trakcyjna"].ToString() == "N")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbProbaTrakcyjnaNie.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu
                }
                else if (row["Proba_trakcyjna"].ToString() == "T")
                {
                    rbProbaTrakcyjnaTak.IsChecked = true;
                }

                if (row["Proba_trakcyjna_symetria"].ToString() == "N")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbProbTrakSymNie.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu
                }
                else if (row["Proba_trakcyjna_symetria"].ToString() == "T")
                {
                    rbProbTrakSymTak.IsChecked = true;
                }

                if (row["Zawieszenie_pronacyjne"].ToString() == "N")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbZawProNie.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu
                }
                else if (row["Zawieszenie_pronacyjne"].ToString() == "T")
                {
                    rbZawProTak.IsChecked = true;
                }

                if (row["Zawieszenie_pronacyjne_symetria"].ToString() == "N")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbZawPronSymNie.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu
                }
                else if (row["Zawieszenie_pronacyjne_symetria"].ToString() == "T")
                {
                    rbZawPronSymTak.IsChecked = true;
                }

                if (row["Zawieszenie_boczne_L"].ToString() == "N")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbZawBoczLNie.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu
                }
                else if (row["Zawieszenie_boczne_L"].ToString() == "T")
                {
                    rbZawBoczLTak.IsChecked = true;
                }

                if (row["Zawieszenie_boczne_P"].ToString() == "N")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbZawBoczPNie.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu
                }
                else if (row["Zawieszenie_boczne_P"].ToString() == "T")
                {
                    rbZawBoczPTak.IsChecked = true;
                }

                if (row["Zawieszenie_boczne_asymetryczne"].ToString() == "L")        //Sprawdzam czy w bazie zapisano tak 1 czy 0 nie
                {
                    rbZawBoczAsL.IsChecked = true;       //i zaznaczam odpowiednie pole w formularzu jeśli 0 to lewa
                }
                else if (row["Zawieszenie_boczne_asymetryczne"].ToString() == "P")
                {
                    rbZawBoczAsP.IsChecked = true;      //1 to prawa
                }

                txtProPoPor.Text = row["Problemy_po_porodzie"].ToString();
                txtDane.Text = row["Wypisy_badania_medyczne"].ToString();
                txtProRodz.Text = row["Zauwazone_problemy"].ToString();
                txtInfPrzetSens.Text = row["Informacje_przetwarzanie"].ToString();
                txtInneSchorz.Text = row["Inne_schorzenia"].ToString();

                txtPozSupi.Text = row["Pozycja_supinacji"].ToString();
                txtPozPron.Text = row["Pozycja_pronacyjna"].ToString();
                txtPozBoczL.Text = row["Pozycja_boczna_L"].ToString();
                txtPozBoczP.Text = row["Pozycja_boczna_P"].ToString();
                txtPozBoczP.Text = row["Pozycja_boczna_P"].ToString();
                txtPozSiedz.Text = row["Pozycja_siedzaca"].ToString();
                txtKlekPod.Text = row["Klek_podparty"].ToString();
                txtKlekJedno.Text = row["Klek_jednonoz"].ToString();
                txtPozStoj.Text = row["Pozycja_stojaca"].ToString();
                txtChod.Text = row["Chod"].ToString();
            }
        }

        private void TxtProWtrakCia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Problemy_ciaza"] = txtProWtrakCia.Text;
            }
        }

        private void RbWterminie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Porod_w_terminie"] = "T";
                row["Porod_poza_terminem"] = "N";
            }
        }

        private void RbPozaTerminem_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Porod_w_terminie"] = "N";
                row["Porod_poza_terminem"] = "T";
            }
        }

        private void RbSilNatTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Porod_silami_natury"] = "T";
            }
        }

        private void RbSilNatNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Porod_silami_natury"] = "N";
            }
        }

        private void RbPlanowane_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Cesarskie_ciecie_planowane"] = "T";
                row["Cesarskie_ciecie_koniecznosc"] = "N";
            }
        }

        private void RbKoniecznosc_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Cesarskie_ciecie_planowane"] = "N";
                row["Cesarskie_ciecie_koniecznosc"] = "T";
            }
        }

        private void TxtProPoPor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Problemy_po_porodzie"] = txtProPoPor.Text;
            }
        }

        private void TxtDane_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Wypisy_badania_medyczne"] = txtDane.Text;
            }
        }

        private void TxtProRodz_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zauwazone_problemy"] = txtProRodz.Text;
            }
        }

        private void TxtInfPrzetSens_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Informacje_przetwarzanie"] = txtInfPrzetSens.Text;
            }
        }

        private void TxtInneSchorz_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Inne_schorzenia"] = txtInneSchorz.Text;
            }
        }

        private void TxtPozSupi_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Pozycja_supinacji"] = txtPozSupi.Text;
            }
        }

        private void TxtPozPron_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Pozycja_pronacyjna"] = txtPozPron.Text;
            }
        }

        private void TxtPozBoczL_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Pozycja_boczna_L"] = txtPozBoczL.Text;
            }
        }

        private void TxtPozBoczP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Pozycja_boczna_P"] = txtPozBoczP.Text;
            }
        }

        private void TxtPozSiedz_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Pozycja_siedzaca"] = txtPozSiedz.Text;
            }
        }

        private void TxtKlekPod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Klek_podparty"] = txtKlekPod.Text;
            }
        }

        private void TxtKlekJedno_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Klek_jednonoz"] = txtKlekJedno.Text;
            }
        }

        private void TxtPozStoj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Pozycja_stojaca"] = txtPozStoj.Text;
            }
        }

        private void TxtChod_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Chod"] = txtChod.Text;
            }
        }

        private void RbProbaTrakcyjnaTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Proba_trakcyjna"] = "T";
            }
        }

        private void RbProbaTrakcyjnaNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Proba_trakcyjna"] = "N";
            }
        }

        private void RbProbTrakSymTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Proba_trakcyjna_symetria"] = "T";
            }
        }

        private void RbProbTrakSymNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Proba_trakcyjna_symetria"] = "N";
            }
        }

        private void RbZawProTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_pronacyjne"] = "T";
            }
        }

        private void RbZawProNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_pronacyjne"] = "N";
            }
        }

        private void RbZawPronSymTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_pronacyjne_symetria"] = "T";
            }
        }

        private void RbZawPronSymNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_pronacyjne_symetria"] = "N";
            }
        }

        private void RbZawBoczLTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_boczne_L"] = "T";
            }
        }

        private void RbZawBoczLNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_boczne_L"] = "N";
            }
        }

        private void RbZawBoczPTak_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_boczne_P"] = "T";
            }
        }

        private void RbZawBoczPNie_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_boczne_P"] = "N";
            }
        }

        private void RbZawBoczAsL_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_boczne_asymetryczne"] = "L";
            }
        }

        private void RbZawBoczAsP_Checked(object sender, RoutedEventArgs e)
        {
            if (row == null)
            {
                MessageBox.Show("Nie wybrano wiersza do edycji!");
            }
            else
            {
                row["Zawieszenie_boczne_asymetryczne"] = "P";
            }
        }

        private void Zapisz()
        {
            MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);      //Nowy obiekt klasy CommandBuilder, który ułoży sparametryzowane zapytanie (update,insert,delet) do bazy z przekazanym dataAdapter

            try         //Próba wykonania aktualizacji danych
            {
                i = dgDaneMedyczne.SelectedIndex;
                dataAdapter.Update(ds, "PacjenciMedyczne");      //przy użyciu metody Update klasy DataAdapter na ds tabeli Wizyty

                ds.Tables["PacjenciMedyczne"].Clear();       //Wyczyszczenie tabeli ze starych danych
                dataAdapter.Fill(ds, "PacjenciMedyczne");        //i ponowne jej wypełnienie

                dgDaneMedyczne.SelectedIndex = i;

                if (ds.Tables["PacjenciMedyczne"].Rows.Count == 0)      //Jeżeli w danych medycznych nic nie ma to z automatu po wybraniu tak zapisz zmiany
                {
                    dodajDaneMedyczne();        //dodawane są jedne dane medyczne
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

        private void LblPorod_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)       //Odznacza radioButtony po kliknięciu w etykietę
        {
            if (row == null)
            {
                rbWterminie.IsChecked = false;
                rbPozaTerminem.IsChecked = false;
                rbSilNatNie.IsChecked = false;
                rbSilNatTak.IsChecked = false;
                rbKoniecznosc.IsChecked = false;
                rbPlanowane.IsChecked = false;
            }
            else
            {
                rbWterminie.IsChecked = false;
                rbPozaTerminem.IsChecked = false;
                rbSilNatNie.IsChecked = false;
                rbSilNatTak.IsChecked = false;
                rbKoniecznosc.IsChecked = false;
                rbPlanowane.IsChecked = false;

                row["Porod_w_terminie"] = "";
                row["Porod_poza_terminem"] = "";
                row["Porod_silami_natury"] = "";
                row["Cesarskie_ciecie_planowane"] = "";
                row["Cesarskie_ciecie_koniecznosc"] = "";
            }
        }

        private void LblProbaTrakcyjna_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)       //Czyszczenie radiobuttonów po kliknieiu w etykiety
        {
            if (row == null)
            {
                rbProbaTrakcyjnaNie.IsChecked = false;
                rbProbaTrakcyjnaTak.IsChecked = false;

                rbProbTrakSymNie.IsChecked = false;
                rbProbTrakSymTak.IsChecked = false;
            }
            else
            {
                rbProbaTrakcyjnaNie.IsChecked = false;
                rbProbaTrakcyjnaTak.IsChecked = false;

                rbProbTrakSymNie.IsChecked = false;
                rbProbTrakSymTak.IsChecked = false;

                row["Proba_trakcyjna"] = "";
                row["Proba_trakcyjna_symetria"] = "";
            }
        }

        private void LblZawPro_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (row == null)
            {
                rbZawProNie.IsChecked = false;
                rbZawProTak.IsChecked = false;

                rbZawPronSymNie.IsChecked = false;
                rbZawPronSymTak.IsChecked = false;
            }
            else
            {
                rbZawProNie.IsChecked = false;
                rbZawProTak.IsChecked = false;

                rbZawPronSymNie.IsChecked = false;
                rbZawPronSymTak.IsChecked = false;

                row["Zawieszenie_pronacyjne"] = "";
                row["Zawieszenie_pronacyjne_symetria"] = "";
            }
        }

        private void LblZawBoczL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (row == null)
            {
                rbZawBoczLTak.IsChecked = false;
                rbZawBoczLNie.IsChecked = false;
            }
            else
            {
                rbZawBoczLTak.IsChecked = false;
                rbZawBoczLNie.IsChecked = false;

                row["Zawieszenie_boczne_L"] = "";
            }
        }

        private void LblZawBoczP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (row == null)
            {
                rbZawBoczPTak.IsChecked = false;
                rbZawBoczPNie.IsChecked = false;
            }
            else
            {
                rbZawBoczPTak.IsChecked = false;
                rbZawBoczPNie.IsChecked = false;

                row["Zawieszenie_boczne_P"] = "";
            }
        }

        private void LblZawBoczAs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (row == null)
            {
                rbZawBoczAsP.IsChecked = false;
                rbZawBoczAsL.IsChecked = false;
            }
            else
            {
                rbZawBoczAsP.IsChecked = false;
                rbZawBoczAsL.IsChecked = false;

                row["Zawieszenie_boczne_asymetryczne"] = "";
            }
        }

        private void TxtInfPrzetSens_GotFocus(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(txtInfPrzetSens, 0);
            Grid.SetRowSpan(txtInfPrzetSens, 7);
        }

        private void TxtInfPrzetSens_LostFocus(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(txtInfPrzetSens, 6);
            Grid.SetRowSpan(txtInfPrzetSens, 1);
        }

        private void TxtProRodz_GotFocus(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(txtProRodz, 3);
            Grid.SetRowSpan(txtProRodz, 3);
        }

        private void TxtProRodz_LostFocus(object sender, RoutedEventArgs e)
        {
            Grid.SetRow(txtProRodz, 5);
            Grid.SetRowSpan(txtProRodz, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Akademia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWyszukiwanie
    {
        private ObservableCollection<Pracownik> listaPracownikow;
        private ObservableCollection<Klient> listaKlientow;
        public MainWindow()
        {
            InitializeComponent();
            listaPracownikow = new ObservableCollection<Pracownik>();
            listaKlientow = new ObservableCollection<Klient>();

            this.ListaPracownikow.ItemsSource = listaPracownikow;
            this.ListaKlientow.ItemsSource = listaKlientow;
            this.Etat.ItemsSource = Enum.GetValues(typeof(Etat));
            this.EtatComboBox.ItemsSource = Enum.GetValues(typeof(Etat));
        }

        private void DodajPracownika(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ImiePracownikaTextBox.Text == "" || NazwiskoPracownikaTextBox.Text == ""
                    || NumerTelefonuPracownikaTextBox.Text == "" || PensjaTextBox.Text == "")
                {
                    Exception pustePole = new Exception("Musisz uzupełnić wszystkie pola!");
                    throw pustePole;
                }
                Pracownik pracownik = new Pracownik();
                pracownik.Imie = ImiePracownikaTextBox.Text;
                pracownik.Nazwisko = NazwiskoPracownikaTextBox.Text;

                string nrTel = NumerTelefonuPracownikaTextBox.Text;
                foreach (char s in nrTel)
                {
                    if (s < '0' || s > '9')
                    {
                        Exception niewlasciwyNumer = new Exception("Numer telefonu ma niewłaściwy format!");
                        throw niewlasciwyNumer;
                    }
                }

                pracownik.NrTelefonu = Convert.ToInt32(NumerTelefonuPracownikaTextBox.Text);
                pracownik.Etat = (Etat)Enum.Parse(typeof(Etat), Etat.Text);

                string pensja = PensjaTextBox.Text;
                foreach (var s in pensja)
                {
                    if (s < '0' || s > '9')
                    {
                        if (s != ',')
                        {
                            if (s == '.')
                            {
                                Exception niewlasciwaPensjaKropka = new Exception("Wpisz przecinek zamiast kropki w pensji!");
                                throw niewlasciwaPensjaKropka;
                            }
                            Exception niewlasciwaPensja = new Exception("Pensja ma niewłaściwy format!");
                            throw niewlasciwaPensja;
                        }
                    }
                }

                pracownik.Pensja = Convert.ToDouble(PensjaTextBox.Text);
                pracownik.DataPrzyjecia = DataPrzyjecia.SelectedDate;
                listaPracownikow.Add(pracownik);
                ImiePracownikaTextBox.Clear();
                NazwiskoPracownikaTextBox.Clear();
                NumerTelefonuPracownikaTextBox.Clear();
                Etat.SelectedIndex = 2;
                PensjaTextBox.Clear();
                DataPrzyjecia.SelectedDate = DateTime.Now;
                MessageBox.Show("Dodano!");
            }
            catch (Exception wyjatek)
            {
                MessageBox.Show(wyjatek.Message);
            }
        }

        private void DodajKlienta(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ImieKlientaTextBox.Text == "" || NazwiskoKlientaTextBox.Text == "" || AdresKlientaTextBox.Text == ""
                    || Pesel.Text == "" || NumerTelefonuKlientaTextBox.Text == "")
                {
                    Exception pustePole = new Exception("Musisz uzupełnić wszystkie pola!");
                    throw pustePole;
                }

                Klient klient = new Klient();
                klient.Imie = ImieKlientaTextBox.Text;
                klient.Nazwisko = NazwiskoKlientaTextBox.Text;

                string nrTel = NumerTelefonuKlientaTextBox.Text;
                foreach (char s in nrTel)
                {
                    if (s < '0' || s > '9')
                    {
                        Exception niewlasciwyNumer = new Exception("Numer telefonu ma niewłaściwy format!");
                        throw niewlasciwyNumer;
                    }
                }

                klient.NrTelefonu = Convert.ToInt32(NumerTelefonuKlientaTextBox.Text);
                klient.Adres = AdresKlientaTextBox.Text;

                if (Pesel.Text.Length == 11)
                {
                    string pesel = Pesel.Text;
                    foreach (char s in pesel)
                    {
                        if (s < '0' || s > '9')
                        {
                            Exception niewlasciwyPesel = new Exception("PESEL ma niewłaściwy format!");
                            throw niewlasciwyPesel;
                        }
                    }
                }
                else
                {
                    Exception niewlasciwyPesel = new Exception("PESEL ma niewłaściwy format!");
                    throw niewlasciwyPesel;
                }

                klient.Pesel = Pesel.Text;
                listaKlientow.Add(klient);
                ImieKlientaTextBox.Clear();
                NazwiskoKlientaTextBox.Clear();
                NumerTelefonuKlientaTextBox.Clear();
                AdresKlientaTextBox.Clear();
                Pesel.Clear();
                MessageBox.Show("Dodano!");
            }
            catch (Exception wyjatek)
            {
                MessageBox.Show(wyjatek.Message);
            }
        }

        private void UsunPracownika(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            listaPracownikow.Remove((Pracownik)but.DataContext);
        }

        private void ZapiszPracownikow(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Pracownicy"; //domyslna nazwa pliku
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filePath = dlg.FileName;
                ListToXmlFilePracownicy(filePath);
            }
        }

        private void ListToXmlFilePracownicy(string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Pracownik>));
                serializer.Serialize(sw, listaPracownikow);
            }
        }

        private void WczytajPracownikow(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";
            if (result == true)
            {
                filename = dlg.FileName;
            }
            if (File.Exists(filename))
            {
                XmlFileToListPracownicy(filename);
            }
            else
            {
                MessageBox.Show("Nie ma takiego pliku!");
            }
        }

        private void XmlFileToListPracownicy(string filename)
        {
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    var deserializer = new XmlSerializer(typeof(ObservableCollection<Pracownik>));
                    ObservableCollection<Pracownik> tmpList = (ObservableCollection<Pracownik>)deserializer.Deserialize(sr);
                    foreach (var item in tmpList)
                    {
                        listaPracownikow.Add(item);
                    }
                }
            }
            catch (InvalidOperationException)
            {

                MessageBox.Show("Niepoprawne dane w pliku!");
            }

        }

        private void UsunKlienta(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            listaKlientow.Remove((Klient)but.DataContext);
        }

        private void ZapiszKlientow(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Klienci"; //domyslna nazwa pliku
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filePath = dlg.FileName;
                ListToXmlFileKlienci(filePath);
            }
        }

        private void ListToXmlFileKlienci(string filePath)
        {
            using (var sw = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Klient>));
                serializer.Serialize(sw, listaKlientow);
            }
        }

        private void WczytajKlientow(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";

            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";
            if (result == true)
            {
                filename = dlg.FileName;
            }
            if (File.Exists(filename))
            {
                XmlFileToListKlienci(filename);
            }
            else
            {
                MessageBox.Show("Nie ma takiego pliku!");
            }
        }

        private void XmlFileToListKlienci(string filename)
        {
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    var deserializer = new XmlSerializer(typeof(ObservableCollection<Klient>));
                    ObservableCollection<Klient> tmpList = (ObservableCollection<Klient>)deserializer.Deserialize(sr);
                    foreach (var item in tmpList)
                    {
                        listaKlientow.Add(item);
                    }
                }
            }
            catch (InvalidOperationException)
            {

                MessageBox.Show("Niepoprawne dane w pliku!");
            }
        }

        private void WyszukajKlient(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Klient> wyszukane = new ObservableCollection<Klient>();
            if (NrTelefonuSzukajKlient.Text != "")
            {
                int nrTelefonuSzukaj = Convert.ToInt32(NrTelefonuSzukajKlient.Text);
                wyszukane = Wyszukaj(listaKlientow, nrTelefonuSzukaj);
                WyszukaniKlienci.ItemsSource = wyszukane;
                return;
            }

            if (NazwiskoSzukajKlient.Text != "")
            {
                wyszukane = Wyszukaj(listaKlientow, NazwiskoSzukajKlient.Text);
                WyszukaniKlienci.ItemsSource = wyszukane;
                return;
            }
        }

        private void NrTelefonuSzukaj_GotFocus(object sender, RoutedEventArgs e)
        {
            NazwiskoSzukajKlient.Text = "";
            NazwiskoSzukajPracownik.Text = "";
        }

        private void NazwiskoSzukaj_GotFocus(object sender, RoutedEventArgs e)
        {
            NrTelefonuSzukajKlient.Text = "";
            NrTelefonuSzukajPracownik.Text = "";
        }

        private void WyszukajPracownik(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Pracownik> wyszukane = new ObservableCollection<Pracownik>();
            if (NrTelefonuSzukajPracownik.Text != "")
            {
                int nrTelefonuSzukaj = Convert.ToInt32(NrTelefonuSzukajPracownik.Text);
                wyszukane = Wyszukaj(listaPracownikow, nrTelefonuSzukaj);
                WyszukaniPracownicy.ItemsSource = wyszukane;
                return;
            }

            if (NazwiskoSzukajPracownik.Text != "")
            {
                wyszukane = Wyszukaj(listaPracownikow, NazwiskoSzukajPracownik.Text);
                WyszukaniPracownicy.ItemsSource = wyszukane;
                return;
            }
        }

        public ObservableCollection<Klient> Wyszukaj(ObservableCollection<Klient> lista, string nazwisko)
        {
            ObservableCollection<Klient> pasujaceOsoby = new ObservableCollection<Klient>();

            foreach (var osoba in lista)
            {
                if (osoba.Nazwisko == nazwisko)
                {
                    pasujaceOsoby.Add(osoba);
                }
            }

            return pasujaceOsoby;
        }

        public ObservableCollection<Klient> Wyszukaj(ObservableCollection<Klient> lista, int nrTelefonu)
        {
            ObservableCollection<Klient> pasujaceOsoby = new ObservableCollection<Klient>();

            foreach (var osoba in lista)
            {
                if (osoba.NrTelefonu == nrTelefonu)
                {
                    pasujaceOsoby.Add(osoba);
                }
            }
            return pasujaceOsoby;
        }

        public ObservableCollection<Pracownik> Wyszukaj(ObservableCollection<Pracownik> lista, string nazwisko)
        {
            ObservableCollection<Pracownik> pasujaceOsoby = new ObservableCollection<Pracownik>();

            foreach (var osoba in lista)
            {
                if (osoba.Nazwisko == nazwisko)
                {
                    pasujaceOsoby.Add(osoba);
                }
            }

            return pasujaceOsoby;
        }

        public ObservableCollection<Pracownik> Wyszukaj(ObservableCollection<Pracownik> lista, int nrTelefonu)
        {
            ObservableCollection<Pracownik> pasujaceOsoby = new ObservableCollection<Pracownik>();

            foreach (var osoba in lista)
            {
                if (osoba.NrTelefonu == nrTelefonu)
                {
                    pasujaceOsoby.Add(osoba);
                }
            }
            return pasujaceOsoby;
        }
    }
}

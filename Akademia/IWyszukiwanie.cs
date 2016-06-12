using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akademia
{
    interface IWyszukiwanie
    {
        ObservableCollection<Klient> Wyszukaj(ObservableCollection<Klient> lista, string nazwisko);
        ObservableCollection<Klient> Wyszukaj(ObservableCollection<Klient> lista, int nrTelefonu);

        ObservableCollection<Pracownik> Wyszukaj(ObservableCollection<Pracownik> lista, string nazwisko);
        ObservableCollection<Pracownik> Wyszukaj(ObservableCollection<Pracownik> lista, int nrTelefonu);
    }
}

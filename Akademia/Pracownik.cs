using System;

namespace Akademia
{
    public enum Etat
    {
        Polowa,
        Trzy_Czwarte,
        Caly
    }

    public class Pracownik : Osoba
    {
        public Etat Etat { get; set; }
        public double Pensja { get; set; }
        public DateTime? DataPrzyjecia { get; set; }

    }
}

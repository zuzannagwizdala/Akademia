using System;

namespace Akademia
{
    //enum
    public enum Etat
    {
        Polowa,
        Trzy_Czwarte,
        Caly
    }

    //klasa2
    //dziedziczenie
    public class Pracownik : Osoba
    {
        //propercje
        public Etat Etat { get; set; }
        public double Pensja { get; set; }
        public DateTime? DataPrzyjecia { get; set; }

    }
}

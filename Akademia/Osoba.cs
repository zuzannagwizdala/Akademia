using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akademia
{
    //klasa1
    public abstract class Osoba
    {
        //propercje
        public virtual string Nazwisko { get; set; }
        public virtual string Imie { get; set; }
        public virtual int NrTelefonu { get; set; }
    }
}

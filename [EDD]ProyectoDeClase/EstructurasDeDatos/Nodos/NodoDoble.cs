using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos
{
    public class NodoDoble<T>
    {
        public T Value { get; set; }
        public NodoSimple<T> Prev { get; set; }
        public NodoSimple<T> Next { get; set; }
    }
}

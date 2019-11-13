using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos
{
    public class NodoSimple<T>
    {
        public T Value { get; set; }
        public NodoSimple<T> Next { get; set; }

        public NodoSimple(T value)
        {
            Value = value;
        }
    }
}

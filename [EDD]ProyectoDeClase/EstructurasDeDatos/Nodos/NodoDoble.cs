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
        /// <summary>
        /// Funciona como prev tambien
        /// </summary>
        public NodoDoble<T> Left { get; set; }
        /// <summary>
        /// Funciona como next tambien
        /// </summary>
        public NodoDoble<T> Right { get; set; }

        public NodoDoble(T value)
        {
            Value = value;
        }
    }
}

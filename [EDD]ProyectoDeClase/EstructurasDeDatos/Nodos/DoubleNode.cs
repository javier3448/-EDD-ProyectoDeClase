using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos
{
    public class DoubleNode<T>
    {
        public T Value { get; set; }
        /// <summary>
        /// Funciona como prev tambien
        /// </summary>
        public DoubleNode<T> Left { get; set; }
        /// <summary>
        /// Funciona como next tambien
        /// </summary>
        public DoubleNode<T> Right { get; set; }

        public DoubleNode(T value)
        {
            Value = value;
        }

        public string GetDotLabel()
        {
            return Value.ToString();
        }
    }
}

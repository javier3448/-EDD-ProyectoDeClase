using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos
{
    public class SimpleNode<T>
    {
        public T Value { get; set; }
        public SimpleNode<T> Next { get; set; }

        public SimpleNode(T value)
        {
            Value = value;
        }

        public string GetDotLabel()
        {
            return Value.ToString();
        }
    }
}

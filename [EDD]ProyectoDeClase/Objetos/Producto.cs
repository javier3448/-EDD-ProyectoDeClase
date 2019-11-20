using _EDD_ProyectoDeClase.EstructurasDeDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.Objetos
{
    public class Producto : Indexable<string, string>
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Proveedor { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }

        public Producto(string nombre, string categoria, string proveedor, double precio, int cantidad)
        {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Categoria = categoria ?? throw new ArgumentNullException(nameof(categoria));
            Proveedor = proveedor ?? throw new ArgumentNullException(nameof(proveedor));
            Precio = precio;
            Cantidad = cantidad;
        }

        public string GetRow()
        {
            return Categoria;
        }

        public string GetColumn()
        {
            return Nombre;
        }

        public override string ToString()
        {
            return string.Format("row: {0}\\ncolumn: {1}\\nProveedor: {2}", Categoria, Nombre, Proveedor);
        }
    }
}

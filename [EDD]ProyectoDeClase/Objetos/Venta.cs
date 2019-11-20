using _EDD_ProyectoDeClase.EstructurasDeDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.Objetos
{
    public class Venta
    {
        public Cliente Cliente { get; set; }
        public DoubleList<Producto> Productos { get; set; } = new DoubleList<Producto>();
        public DoubleList<IntWrapper> CantidadDeProducto { get; set; } = new DoubleList<IntWrapper>();
        public DateTime Fecha { get; set; }

        public Venta(Cliente cliente, DateTime fecha)
        {
            Cliente = cliente;
            Fecha = fecha;
        }

        public Venta()
        {
            Cliente = null;
            Fecha = DateTime.Today;
        }

        public double GetTotal()
        {
            var total = 0.0;
            int i = 0;
            foreach (var producto in Productos)
            {
                total += producto.Precio * CantidadDeProducto.Get(i).IntValue;
                i++;
            }
            return total;
        }
        public Venta(Cliente cliente, DoubleList<Producto> productos, DateTime fecha)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Productos = productos ?? throw new ArgumentNullException(nameof(productos));
            Fecha = fecha;
        }

        public void AddProducto(Producto producto, int cantidad)
        {
            int i = 0;
            foreach (var p in Productos)
            {
                if (p.Equals(producto))
                {
                    CantidadDeProducto.Get(i).IntValue += cantidad;
                    return;
                }
                i++;
            }
            Productos.AddLast(producto);
            CantidadDeProducto.AddLast(new IntWrapper(cantidad));
        }
    }
}

using _EDD_ProyectoDeClase.EstructurasDeDatos;
using _EDD_ProyectoDeClase.Objetos;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _EDD_ProyectoDeClase.MyGui
{
    public partial class MainForm : Form
    {
        public Matrix<string, string, Producto> Productos = new Matrix<string, string, Producto>();
        public DoubleList<Cliente> Clientes = new DoubleList<Cliente>();
        public static int CuentaDeVentas = 255;
        public MyHashtable<MyString, Venta> Ventas = new MyHashtable<MyString, Venta>();
        public Venta VentaActual = new Venta();

        public MainForm()
        {
            InitializeComponent();
            InitEstructuras();
        }

        private void InitEstructuras()
        {
            Clientes.AddFirst(Cliente.ConsumidorFinal);
        }

        private void buttonAgregarProducto_Click(object sender, EventArgs e)
        {
            var nombre = textBoxAgregarProductoNombre.Text;
            var categoria = textBoxAgregarProductoCategoria.Text;
            var proveedor = textBoxAgregarProductoProveedor.Text;
            var precio = textBoxAgregarProductoPrecio.Text;
            var cantidad = textBoxAgregarProductoCantidad.Text;

            if (nombre.IsEmpty() || categoria.IsEmpty() || proveedor.IsEmpty() || !precio.IsNumber() || !cantidad.IsInt())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var producto = new Producto(nombre, categoria, proveedor, Convert.ToDouble(precio), Convert.ToInt32(cantidad));

            var result = Productos.Add(producto);

            if (!result)
            {
                MessageBox.Show("Ya existe un producto con el mismo nombre y categoria en el inventario");
            }
            else
            {
                MessageBox.Show("El producto se agrego exitosamente");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBoxBuscarProductoNombre.Items.Clear();
            comboBoxBuscarProductoCategoria.Items.Clear();

            var nombres = Productos.GetRows();
            var categorias = Productos.GetColumns();
            foreach (var nombre in nombres)
            {
                comboBoxBuscarProductoNombre.Items.Add(nombre.ToString());
            }
            foreach (var categoria in categorias)
            {
                comboBoxBuscarProductoCategoria.Items.Add(categoria.ToString());
            }
        }

        private void buttonBuscarProducto_Click(object sender, EventArgs e)
        {
            var nombre = (comboBoxBuscarProductoNombre.SelectedItem ?? "").ToString();
            var categoria = (comboBoxBuscarProductoCategoria.SelectedItem ?? "").ToString();

            if (nombre.IsEmpty() || categoria.IsEmpty())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var producto = Productos.Get(nombre, categoria);
            if (producto == null)
            {
                MessageBox.Show("No existe un producto con ese nombre y categoria");
                return;
            }

            //Llenar los text boxes
            textBoxEditarProductoNombre.Text = nombre;
            textBoxEditarProductoCategoria.Text = categoria;
            textBoxEditarProductoProveedor.Text = producto.Proveedor;
            textBoxEditarProductoPrecio.Text = producto.Precio.ToString();
            textBoxEditarProductoCantidad.Text = producto.Cantidad.ToString();
        }

        private void buttonActualizarProductos_Click(object sender, EventArgs e)
        {
            var dotSrc = Productos.GetDot();
            Debug.WriteLine(dotSrc);
            var img = Graphviz.DotCompiler.SavePng("res" + Path.DirectorySeparatorChar + "productos.png", dotSrc);
            if (img == null)
            {
                MessageBox.Show("No se pudo compilar el codigo dot!");
            }
            pictureBox1.Image = img;
        }

        private void buttonEditarProducto_Click(object sender, EventArgs e)
        {
            var nombre = (comboBoxBuscarProductoNombre.SelectedItem ?? "").ToString();
            var categoria = (comboBoxBuscarProductoCategoria.SelectedItem ?? "").ToString();

            if (nombre.IsEmpty() || categoria.IsEmpty())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var producto = Productos.Get(nombre, categoria);
            if (producto == null)
            {
                MessageBox.Show("No existe un producto con ese nombre y categoria");
                return;
            }

            var proveedorNuevo = textBoxEditarProductoProveedor.Text;
            var precioNuevo = textBoxEditarProductoPrecio.Text;
            var cantidadNueva = textBoxEditarProductoCantidad.Text;
            
            if (proveedorNuevo.IsEmpty() || !precioNuevo.IsNumber() || !cantidadNueva.IsInt())
            {
                MessageBox.Show("Datos nuevos no validos");
                return;
            }

            producto.Proveedor = proveedorNuevo;
            producto.Precio = Convert.ToDouble(precioNuevo);
            producto.Cantidad = Convert.ToInt32(cantidadNueva);

            MessageBox.Show("Producto actualizado exitosamente!");
        }

        private void buttonAgregarCliente_Click(object sender, EventArgs e)
        {
            var nit = textBoxAgregarClienteNit.Text;
            var nombre = textBoxAgregarClienteNombre.Text;
            var direccion = textBoxAgregarClienteDireccion.Text;

            if (nit.IsEmpty() || nombre.IsEmpty() || direccion.IsEmpty())
            {
                MessageBox.Show("Datos nuevos no validos");
                return;
            }

            var cliente = new Cliente(nit, nombre, direccion);

            if (Clientes.Contains(cliente))
            {
                MessageBox.Show("Ya existe un cliente con el mismo");
                return;
            }

            Clientes.AddLast(cliente);

            MessageBox.Show("Cliente agregado exitosamente!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBoxBuscarClienteNit.Items.Clear();

            foreach (var cliente in Clientes)
            {
                comboBoxBuscarClienteNit.Items.Add(cliente.Nit);
            }
        }

        private void buttonBuscarCliente_Click(object sender, EventArgs e)
        {
            var nit = comboBoxBuscarClienteNit.SelectedItem.ToString();
            if (nit.IsEmpty())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var cliente = Clientes.Get(new Cliente(nit));
            if (cliente == null)
            {
                MessageBox.Show("No existe un producto con ese nombre y categoria");
                return;
            }

            //Llenar los text boxes
            textBoxEditarClienteNit.Text = nit;
            textBoxEditarClienteNombre.Text = cliente.Nombre;
            textBoxEditarClienteDireccion.Text = cliente.Direccion;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var nit = textBoxEditarClienteNit.Text;
            if (nit.IsEmpty())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var cliente = Clientes.Get(new Cliente(nit));
            if (cliente == null)
            {
                MessageBox.Show("No existe un producto con ese nombre y categoria");
                return;
            }
            if (textBoxEditarClienteNit.Text.IsEmpty() || textBoxEditarClienteNombre.Text.IsEmpty() || textBoxEditarClienteDireccion.Text.IsEmpty())
            {
                MessageBox.Show("Datos nuevos no validos");
                return;
            }
            //Llenar los text boxes
            cliente.Nombre = textBoxEditarClienteNombre.Text;
            cliente.Direccion = textBoxEditarClienteDireccion.Text;
        }

        private void buttonActualizarClientes_Click(object sender, EventArgs e)
        {
            var dotSrc = Clientes.GetDot();
            Debug.WriteLine(dotSrc);
            var img = Graphviz.DotCompiler.SavePng("res" + Path.DirectorySeparatorChar + "clientes.png", dotSrc);
            if (img == null)
            {
                MessageBox.Show("No se pudo compilar el codigo dot!");
            }
            pictureBox2.Image = img;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            comboBoxElimnarClienteNit.Items.Clear();

            foreach (var cliente in Clientes)
            {
                comboBoxElimnarClienteNit.Items.Add(cliente.Nit);
            }
        }

        private void buttonEliminarCliente_Click(object sender, EventArgs e)
        {
            var nit = comboBoxBuscarClienteNit.SelectedItem.ToString();
            if (nit.IsEmpty())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            if (Clientes.Delete(new Cliente(nit)))
            {
                MessageBox.Show("Cliente eliminado exitosamente");
            }
            else
            {
                MessageBox.Show("No existe un cliente con ese nit");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBoxAgregarVentaCliente.Items.Clear();
            comboBoxAgregarVentaNombre.Items.Clear();
            comboBoxAgregarVentaCategoria.Items.Clear();

            foreach (var cliente in Clientes)
            {
                comboBoxAgregarVentaCliente.Items.Add(cliente.Nit);
            }

            var nombres = Productos.GetRows();
            var categorias = Productos.GetColumns();
            foreach (var nombre in nombres)
            {
                comboBoxAgregarVentaNombre.Items.Add(nombre.ToString());
            }
            foreach (var categoria in categorias)
            {
                comboBoxAgregarVentaCategoria.Items.Add(categoria.ToString());
            }

            ActualizarVistaPrevia();
        }

        private void buttonCancelarVenta_Click(object sender, EventArgs e)
        {
            VentaActual = new Venta();
            ActualizarVistaPrevia();
        }

        private void buttonAgregarProductoAVenta_Click(object sender, EventArgs e)
        {
            var nombre = (comboBoxAgregarVentaNombre.SelectedItem ?? "").ToString();
            var categoria = (comboBoxAgregarVentaCategoria.SelectedItem ?? "").ToString();
            var cantidad = textBoxAgregarVentaCantidad.Text;

            if (nombre.IsEmpty() || categoria.IsEmpty() || !cantidad.IsInt())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var producto = Productos.Get(nombre, categoria);

            if (producto == null)
            {
                MessageBox.Show("No existe el producto en la matriz");
                return;
            }

            var date = monthCalendar1.SelectionRange.Start;

            VentaActual.AddProducto(producto, Convert.ToInt32(cantidad));

            ActualizarVistaPrevia();
        }

        private void buttonAgregarVenta_Click(object sender, EventArgs e)
        {
            var clienteNit = (comboBoxAgregarVentaCliente.SelectedItem ?? "").ToString();

            if (clienteNit.IsEmpty())
            {
                MessageBox.Show("Datos no validos");
                return;
            }

            var cliente = Clientes.Get(new Cliente(clienteNit));
            if (cliente == null)
            {
                MessageBox.Show("No existe ese cliente");
                MessageBox.Show("No existe ese cliente");
                return;
            }

            var date = monthCalendar1.SelectionRange.Start;

            if (date == null)
            {
                date = DateTime.Today;
            }

            VentaActual.Fecha = date;
            VentaActual.Cliente = cliente;

            var ventaId = new MyString(CuentaDeVentas.ToString());
            Ventas.Add(ventaId, VentaActual);
            CuentaDeVentas++;

            ActualizarVistaPrevia();
            MessageBox.Show("Se agrego la venta exitosamente");
            VentaActual = new Venta();
        }
        private void ActualizarVistaPrevia()
        {
            var sb = new StringBuilder("------------------------------------------\r\n" + 
                                       "*****************El Chino*****************\r\n" + 
                                       "------------------------------------------\r\n" +
                                       "          Factura de pequeño Contribuyente\r\n" +
                                       "                                 No. " + CuentaDeVentas.ToString("00000") + "\r\n"
                                       );
            sb.Append("Fecha: " + VentaActual.Fecha.ToString("dddd, dd MMMM yyyy") + "\r\n");
            var nombreCliente = VentaActual.Cliente == null ? "" : VentaActual.Cliente.Nombre;
            sb.Append("Nombre: " + nombreCliente + "\r\n");
            var nitCliente = VentaActual.Cliente == null ? "" : VentaActual.Cliente.Nit;
            sb.Append("NIT: " + nitCliente + "\r\n");
            var direccionCliente = VentaActual.Cliente == null ? "" : VentaActual.Cliente.Direccion;
            sb.Append("Direccion: " + direccionCliente + "\r\n");
            sb.Append("\r\n");
            sb.Append("Cantidad   Nombre              Valor      " + "\r\n");

            int i = 0;
            foreach (var producto in VentaActual.Productos)
            {
                int cantidad = VentaActual.CantidadDeProducto.Get(i).IntValue;
                sb.Append(cantidad.ToString().Fit(10) + "|" +
                    producto.Nombre.Fit(19) + "|" +
                    (producto.Precio * cantidad).ToString("#################.00").Fit(11) + "\r\n"
                    );
            }

            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append("Total: " + VentaActual.GetTotal().ToString("#################.00"));

            textBoxVentaVistaPrevia.Text = sb.ToString();
        }
    }

    public static class MyExtensions
    {
        public static bool IsNumber(this string s)
        {
            return Regex.IsMatch(s, "^[0-9]+(\\.[0-9]+)?$");
        }

        public static bool IsInt(this string s)
        {
            return Regex.IsMatch(s, "^[0-9]+$");
        }

        public static bool IsEmpty(this string s)
        {
            return s == "";
        }

        public static string Fit(this string s, int length)
        {
            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            var sb = new StringBuilder(s);
            while (length > sb.Length)
            {
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}

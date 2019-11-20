using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.Objetos
{
    public class Cliente
    {
        public string Nit { get; set; } //Si es null es consumidor final
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        public static Cliente ConsumidorFinal { get; } = new Cliente("Consumidor final", "Consumidor final", null);

        public Cliente(string nit, string name, string address)
        {
            Nit = nit;
            Nombre = name;
            Direccion = address;
        }

        public Cliente(string nit)
        {
            Nit = nit;
        }

        public override bool Equals(object obj)
        {
            return obj is Cliente cliente &&
                   Nit == cliente.Nit;
        }

        public override int GetHashCode()
        {
            var hashCode = -1162250579;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Nit);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Nombre);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Direccion);
            return hashCode;
        }

        public override string ToString()
        {
            return String.Format("Nit: {0}\\nNombre: {1}\\nDireccion: {2}", Nit, Nombre, Direccion); ;
        }
    }
}

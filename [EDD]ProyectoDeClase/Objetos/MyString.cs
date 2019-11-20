using _EDD_ProyectoDeClase.EstructurasDeDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.Objetos
{
    public class MyString : IMyHash
    {
        public string StringValue { get; set; }

        public MyString(string stringValue)
        {
            StringValue = stringValue ?? throw new ArgumentNullException(nameof(stringValue));
        }

        public int GetMyHash()
        {
            int hash = 0;
            for (int i = 0; i < StringValue.Length; i++)
            {
                hash += StringValue[i];
            }
            return hash;
        }
    }
}

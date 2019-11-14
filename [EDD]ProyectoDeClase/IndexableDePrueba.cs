using _EDD_ProyectoDeClase.EstructurasDeDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase
{
    class IndexableDePrueba : Indexable<string, string>
    {
        public string Row;
        public string Column;

        public string Value;

        public IndexableDePrueba(string row, string column, string value)
        {
            Row = row ?? throw new ArgumentNullException(nameof(row));
            Column = column ?? throw new ArgumentNullException(nameof(column));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string GetColumn()
        {
            return Row;
        }

        public string GetRow()
        {
            return Column;
        }

        public override string ToString()
        {
            return String.Format("(row: {0} col: {1}){2}", Row, Column, Value);
        }
    }
}

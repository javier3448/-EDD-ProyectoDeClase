using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos
{
    public interface Indexable<RowType, ColumnType>
    {
        RowType GetRow();
        ColumnType GetColumn();
    }
}

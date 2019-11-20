using _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos
{
    public class Matrix <RowType, ColumnType, CellType> : IEnumerable<CellType>
        where RowType : class, IComparable<RowType>
        where ColumnType : class, IComparable<ColumnType>
        where CellType : class, Indexable<RowType, ColumnType>
    {
        private RowNode RowNodeHead;
        private ColumnNode ColumnNodeHead;

        public class RowNode
        {
            public RowType Value { get; set; }
            public RowNode Up { get; set; }
            public RowNode Down { get; set; }
            public CellNode Right { get; set; }

            public RowNode(RowType value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public string GetDotLabel()
            {
                return Value.ToString();
            }
        }

        public class ColumnNode
        {
            public ColumnType Value { get; set; }
            public ColumnNode Left { get; set; }
            public ColumnNode Right { get; set; }
            public CellNode Down { get; set; }

            public ColumnNode(ColumnType value)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }

            public string GetDotLabel()
            {
                return Value.ToString();
            }
        }        

        public class CellNode
        {
            public CellType Value { get; set; }

            public CellNode Left { get; set; }
            public CellNode Right { get; set; }
            public CellNode Up { get; set; }
            public CellNode Down { get; set; }

            public CellNode(CellType value)
            {
                Value = value;
            }

            public string GetDotLabel()
            {
                return Value.ToString();
            }
        }

        public bool Add(CellType entry)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            var entryRow = entry.GetRow();
            var entryColumn = entry.GetColumn();

            var matrixRowNode = GetOrAddRowNode(entryRow);
            var matrixColumnNode = GetOrAddColumnNode(entryColumn);

            var entryNode = new CellNode(entry);

            if (!AddToRow(matrixRowNode, entryNode))
                return false;

            if (!AddToColumn(matrixColumnNode, entryNode))
                return false;

            return true;
        }

        public CellType Get(RowType row, ColumnType column)
        {
            var currRow = RowNodeHead;
            //Conseguir la Row
            while (currRow != null)
            {
                if (currRow.Value.CompareTo(row) == 0)
                    break;
                currRow = currRow.Down;
            }
            if (currRow == null)
                return null;

            var currCell = currRow.Right;
            while (currCell != null)
            {
                if (currCell.Value.GetColumn().CompareTo(column) == 0)
                    return currCell.Value;
            }
            return null;
        }

        public DoubleList<CellType> GetRow(RowType row)
        {
            var currRow = GetRowNode(row);
            var list = new DoubleList<CellType>();
            if (currRow == null)
                return list;

            var currCell = currRow.Right;
            while (currCell != null)
            {
                list.AddLast(currCell.Value);
                currCell = currCell.Right;
            }
            return list;
        }

        private RowNode GetRowNode(RowType row)
        {
            var currRow = RowNodeHead;
            while (currRow != null)
            {
                if (currRow.Value.CompareTo(row) == 0)
                    break;
                currRow = currRow.Down;
            }
            return currRow;
        }

        public DoubleList<CellType> GetColumn(ColumnType column)
        {
            var list = new DoubleList<CellType>();
            var currColumn = GetColumnNode(column);
            if (currColumn == null)
                return list;

            var currCell = currColumn.Down;
            while (currCell != null)
            {
                list.AddLast(currCell.Value);
                currCell = currCell.Down;
            }
            return list;
        }

        private ColumnNode GetColumnNode(ColumnType column)
        {
            var currColumn = ColumnNodeHead;
            while (currColumn != null)
            {
                if (currColumn.Value.CompareTo(column) == 0)
                    break;
                currColumn = currColumn.Right;
            }
            return currColumn;
        }

        public bool Delete(RowType row, ColumnType column)
        {
            var currRow = RowNodeHead;
            //Conseguir la Row
            while (currRow != null)
            {
                if (currRow.Value.CompareTo(row) == 0)
                    break;
                currRow = currRow.Down;
            }
            if (currRow == null)
                return false;

            var currCell = currRow.Right;
            while (currCell != null)
            {
                if (currCell.Value.GetColumn().CompareTo(column) == 0)
                {
                    //Revisa si es el unico elemento en la fila
                    if (currCell.Left == null && currCell.Right == null)
                    {
                        DeleteRow(currRow);
                        return true;
                    }
                    DeleteNode(currRow, currCell);
                    return true;
                }
            }
            return false;
        }

        private void DeleteRow(RowNode rowNode)
        {
            var up = rowNode.Up;
            var down = rowNode.Down;

            if (up == null)//currRow es first
                RowNodeHead = down;
            else
                up.Down = down;

            if (down != null)
                down.Up = up;
        }

        private void DeleteNode(RowNode rowNode, CellNode cellNode)
        {
            //Eliminar de fila
            var left = cellNode.Left;
            var right = cellNode.Right;

            if (left == null && right == null)//Es el unico nodo en la fila
                DeleteRow(rowNode);

            if (left == null)//curr es first
                rowNode.Right = right;
            else
                left.Right = right;

            if (right != null)
                left.Right = right;

            //Eliminar de columna
            var up = cellNode.Up;
            var down = cellNode.Down;

            if (up == null && down == null)//Es el unico nodo en la fila
            {
                var columnNode = GetColumnNode(cellNode.Value.GetColumn());//No se va usar en todos los caso pero igual la vamos a obtener siempre
                DeleteColumn(columnNode);
            }

            if (up == null)//curr es first
            {
                var columnNode = GetColumnNode(cellNode.Value.GetColumn());//No se va usar en todos los caso pero igual la vamos a obtener siempre
                columnNode.Down = down;
            }
            else
            {
                up.Down = down;
            }

            if (down != null)
                down.Up = up;
        }

        private void DeleteColumn(ColumnNode columnNode)
        {
            var left = columnNode.Left;
            var right = columnNode.Right;

            if (left == null)//currRow es first
                ColumnNodeHead = right;
            else
                left.Right = right;

            if (right != null)
                right.Left = left;
        }

        private RowNode GetOrAddRowNode(RowType entryRow)
        {
            var curr = RowNodeHead;
            if (curr == null)
            {
                RowNodeHead = new RowNode(entryRow);
                return RowNodeHead;
            }
            var prevOfCurr = curr.Up;
            RowType currValue;
            int comparisonResult;
            RowNode entryNode;
            while (curr != null)
            {
                currValue = curr.Value;
                comparisonResult = currValue.CompareTo(entryRow);
                if (comparisonResult == 0)
                {
                    return curr;
                }
                else if (comparisonResult > 0)
                {
                    var prevNode = curr.Up;
                    var nextNode = curr;

                    entryNode = new RowNode(entryRow);

                    if (prevNode == null)//El nodo a insertar es first
                    {
                        RowNodeHead = entryNode;
                        entryNode.Up = null;
                    }
                    else
                    {
                        prevNode.Down = entryNode;
                        entryNode.Up = prevNode;
                    }

                    nextNode.Up = entryNode;
                    entryNode.Down = nextNode;

                    return entryNode;
                }
                prevOfCurr = curr;
                curr = curr.Down;
            }
            //Como ahora curr es null tenemos que movernos de regreso a su 'prev'
            curr = prevOfCurr;
            currValue = curr.Value;
            comparisonResult = currValue.CompareTo(entryRow);
            if (comparisonResult == 0)
            {
                return curr;
            }
            entryNode = new RowNode(entryRow);

            curr.Down = entryNode;
            entryNode.Up = curr;

            return entryNode;
        }

        private ColumnNode GetOrAddColumnNode(ColumnType entryColumn)
        {
            var curr = ColumnNodeHead;
            if (curr == null)
            {
                ColumnNodeHead = new ColumnNode(entryColumn);
                return ColumnNodeHead; 
            }
            var prevOfCurr = curr.Left;
            ColumnType currValue;
            int comparisonResult;
            ColumnNode entryNode;
            while (curr != null)
            {
                currValue = curr.Value;
                comparisonResult = currValue.CompareTo(entryColumn);
                if (comparisonResult == 0)
                {
                    return curr;
                }
                else if (comparisonResult > 0)
                {
                    var prevNode = curr.Left;
                    var nextNode = curr;

                    entryNode = new ColumnNode(entryColumn);

                    if (prevNode == null)//El nodo a insertar es first
                    {
                        ColumnNodeHead = entryNode;
                        entryNode.Left = null;
                    }
                    else
                    {
                        prevNode.Right = entryNode;
                        entryNode.Left = prevNode;
                    }

                    nextNode.Left = entryNode;
                    entryNode.Right = nextNode;

                    return entryNode;
                }
                prevOfCurr = curr;
                curr = curr.Right;
            }
            //Como ahora curr es null tenemos que movernos de regreso a su 'prev'
            curr = prevOfCurr;
            currValue = curr.Value;
            comparisonResult = currValue.CompareTo(entryColumn);
            if (comparisonResult == 0)
            {
                return curr;
            }

            entryNode = new ColumnNode(entryColumn);

            curr.Right = entryNode;
            entryNode.Left = curr;

            return entryNode;
        }

        private bool AddToRow(RowNode rowNode, CellNode entryNode)
        {
            var curr = rowNode.Right;
            if (curr == null)
            {
                rowNode.Right = entryNode;
                return true;
            }
            var prevOfCurr = curr.Left;
            ColumnType entryColumn = entryNode.Value.GetColumn();
            ColumnType currColumn;
            int comparisonResult;
            while (curr != null)
            {
                currColumn = curr.Value.GetColumn();
                comparisonResult = currColumn.CompareTo(entryColumn);
                if (comparisonResult == 0)
                {
                    return false;
                }
                else if (comparisonResult > 0)
                {
                    var prevNode = curr.Left;
                    var nextNode = curr;

                    if (prevNode == null)//El nodo a insertar es first
                    {
                        rowNode.Right = entryNode;
                        entryNode.Left = null;
                    }
                    else
                    {
                        prevNode.Right = entryNode;
                        entryNode.Left = prevNode;
                    }

                    nextNode.Left = entryNode;
                    entryNode.Right = nextNode;

                    return true;
                }
                prevOfCurr = curr;
                curr = curr.Right;
            }
            //Como ahora curr es null tenemos que movernos de regreso a su 'prev'
            curr = prevOfCurr;
            currColumn = curr.Value.GetColumn();
            comparisonResult = currColumn.CompareTo(entryColumn);
            if (comparisonResult == 0)
            {
                return false;
            }

            curr.Right = entryNode;
            entryNode.Left = curr;

            return true;
        }

        private bool AddToColumn(ColumnNode columnNode, CellNode entryNode)
        {
            var curr = columnNode.Down;
            if (curr == null)
            {
                columnNode.Down = entryNode;
                return true;
            }
            var prevOfCurr = curr.Up;
            RowType entryRow = entryNode.Value.GetRow();
            RowType currRow;
            int comparisonResult;
            while (curr != null)
            {
                currRow = curr.Value.GetRow();
                comparisonResult = currRow.CompareTo(entryRow);
                if (comparisonResult == 0)
                {
                    return false;
                }
                else if (comparisonResult > 0)
                {
                    var prevNode = curr.Up;
                    var nextNode = curr;

                    if (prevNode == null)//El nodo a insertar es first
                    {
                        columnNode.Down = entryNode;
                        entryNode.Up = null;
                    }
                    else
                    {
                        prevNode.Down = entryNode;
                        entryNode.Up = prevNode;
                    }

                    nextNode.Up = entryNode;
                    entryNode.Down = nextNode;

                    return true;
                }
                prevOfCurr = curr;
                curr = curr.Down;
            }
            //Como ahora curr es null tenemos que movernos de regreso a su 'prev'
            curr = prevOfCurr;
            currRow = curr.Value.GetRow();
            comparisonResult = currRow.CompareTo(entryRow);
            if (comparisonResult == 0)
            {
                return false;
            }

            curr.Down = entryNode;
            entryNode.Up = curr;

            return true;
        }

        public DoubleList<RowType> GetRows()
        {
            var curr = RowNodeHead;
            var list = new DoubleList<RowType>();
            while(curr != null)
            {
                list.AddLast(curr.Value);
                curr = curr.Down;
            }
            return list;
        }

        public DoubleList<ColumnType> GetColumns()
        {
            var curr = ColumnNodeHead;
            var list = new DoubleList<ColumnType>();
            while (curr != null)
            {
                list.AddLast(curr.Value);
                curr = curr.Right;
            }
            return list;
        }

        //Chapuz medio o alto. Creo???. Estudiar mas enumerable
        private IEnumerable<CellType> GetMyEnumerable()
        {
            var currRow = RowNodeHead;
            CellNode currCell;
            while (currRow != null)
            {
                currCell = currRow.Right;
                while (currCell != null)
                {
                    yield return currCell.Value;
                    currCell = currCell.Right;
                }
                currRow = currRow.Down;
            }
        }
        public IEnumerator<CellType> GetEnumerator()
        {
            return GetMyEnumerable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)GetMyEnumerable()).GetEnumerator();
        }

        public string GetDot()
        {
            //Conseguimos la capa con el codigo del parametro

            var sb = new StringBuilder();

            var rankN = new StringBuilder(); //Todos los nodos que deben tener el rank igual a Mt

            sb.Append("digraph Sparce_Matrix{\n");
            sb.Append("node[shape = box];\n");
            sb.Append("Mt[ label = \"Mt\", width = 1.5, style = filled, fillcolor = firebrick1, group = -1 ];\n\n");

            //Hace sus ejes con mt
            if (RowNodeHead != null)
                sb.Append("Mt->" + RowNodeHead.GetHashCode() + ";\n");
            if (ColumnNodeHead != null)
                sb.Append("Mt->" + ColumnNodeHead.GetHashCode() + ";\n");

            var tmpIndice = ColumnNodeHead;
            var groupCount = 0;
            while (tmpIndice != null)
            {
                //Declara el nodo
                sb.Append("" + tmpIndice.GetHashCode() +
                "[ label = \"" + tmpIndice.GetDotLabel() + "\", width = 1.5, style = filled, fillcolor = lightskyblue, group = " +
                groupCount + " ];\n");

                //Hace sus ejes
                if (tmpIndice.Left != null)
                {
                    sb.Append(tmpIndice.GetHashCode() + "->" + tmpIndice.Left.GetHashCode() + ";\n");
                }
                if (tmpIndice.Right != null)
                {
                    sb.Append(tmpIndice.GetHashCode() + "->" + tmpIndice.Right.GetHashCode() + ";\n");
                }
                if (tmpIndice.Down != null)
                {
                    sb.Append(tmpIndice.GetHashCode() + "->" + tmpIndice.Down.GetHashCode() + ";\n");
                }

                //Pone todos los nodos column en rankN para que mas adelante puedan tener el mismo rank que Mt
                rankN.Append(tmpIndice.GetHashCode() + "; ");

                tmpIndice = tmpIndice.Right;
                groupCount++;
            }

            sb.Append("{ rank = same; Mt;" + rankN.ToString() + "}\n");

            var tmpIndiceRow = RowNodeHead; //tmpIndiceRow pasa a ser el indice de las row
            CellNode tmpContenido;
            //Los nodo contendio y nodos filas
            while (tmpIndiceRow != null)
            {
                rankN = new StringBuilder("{ rank = same; ");//pasa a ser el rank de la siguiente fila
                //Hace el nodo de la row
                sb.Append(tmpIndiceRow.GetHashCode() +
                    "[ label = \"" + tmpIndiceRow.GetDotLabel() + "\", width = 1.5, group = " + -1 + " ];\n");

                //Hace sus ejes
                if (tmpIndiceRow.Right != null)
                {
                    sb.Append(tmpIndiceRow.GetHashCode() + "->" + tmpIndiceRow.Right.GetHashCode() + ";\n");
                }
                if (tmpIndiceRow.Up != null)
                {
                    sb.Append(tmpIndiceRow.GetHashCode() + "->" + tmpIndiceRow.Up.GetHashCode() + ";\n");
                }
                if (tmpIndiceRow.Down != null)
                {
                    sb.Append(tmpIndiceRow.GetHashCode() + "->" + tmpIndiceRow.Down.GetHashCode() + ";\n");
                }
                //Lo agrega al rank de la fila actual
                rankN.Append(tmpIndiceRow.GetHashCode() + "; ");

                tmpContenido = tmpIndiceRow.Right;
                groupCount = 0;
                while (tmpContenido != null)
                {
                    //Declara el nodo
                    sb.Append(tmpContenido.GetHashCode() +
                        "[ label = \"" + tmpContenido.GetDotLabel() + "\", width = 1.5, group = " + groupCount + " ];\n");

                    //Hace sus ejes
                    if (tmpContenido.Left != null)
                    {
                        sb.Append(tmpContenido.GetHashCode() + "->"  + tmpContenido.Left.GetHashCode() + ";\n");
                    }
                    if (tmpContenido.Right != null)
                    {
                        sb.Append(tmpContenido.GetHashCode() + "->" + tmpContenido.Right.GetHashCode() + ";\n");
                    }
                    if (tmpContenido.Up != null)
                    {
                        sb.Append(tmpContenido.GetHashCode() + "->" + tmpContenido.Up.GetHashCode() + ";\n");
                    }
                    if (tmpContenido.Down != null)
                    {
                        sb.Append(tmpContenido.GetHashCode() + "->" + tmpContenido.Down.GetHashCode() + ";\n");
                    }

                    //Lo agrega al rank de la fila actual
                    rankN.Append(tmpContenido.GetHashCode() + "; ");

                    tmpContenido = tmpContenido.Right;
                    groupCount++;
                }
                rankN.Append("}");
                sb.Append(rankN.ToString() + "\n");
                tmpIndiceRow = tmpIndiceRow.Down;
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}

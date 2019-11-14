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
            var currRow = RowNodeHead;
            var list = new DoubleList<CellType>();
            //Conseguir la Row
            while (currRow != null)
            {
                if (currRow.Value.CompareTo(row) == 0)
                    break;
                currRow = currRow.Down;
            }
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

        public DoubleList<CellType> GetColumn(ColumnType column)
        {
            var currColumn = ColumnNodeHead;
            var list = new DoubleList<CellType>();
            //Conseguir la Row
            while (currColumn != null)
            {
                if (currColumn.Value.CompareTo(column) == 0)
                    break;
                currColumn = currColumn.Right;
            }
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

        private RowNode GetOrAddRowNode(RowType entryRow)
        {
            var curr = RowNodeHead;
            if (curr == null)
            {
                RowNodeHead = new RowNode(entryRow);
                return RowNodeHead;
            }
            RowType currValue;
            int comparisonResult;
            RowNode entryNode;
            while (curr.Down != null)
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

                    prevNode.Down = entryNode;
                    entryNode.Up = prevNode;

                    nextNode.Up = entryNode;
                    entryNode.Down = nextNode;

                    return entryNode;
                }
                curr = curr.Down;
            }
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
            ColumnType currValue;
            int comparisonResult;
            ColumnNode entryNode;
            while (curr.Right != null)
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

                    prevNode.Left = entryNode;
                    entryNode.Right = prevNode;

                    nextNode.Left = entryNode;
                    entryNode.Right = nextNode;

                    return entryNode;
                }
                curr = curr.Right;
            }
            currValue = curr.Value;
            comparisonResult = currValue.CompareTo(entryColumn);
            if (comparisonResult == 0)
            {
                return curr;
            }
            entryNode = new ColumnNode(entryColumn);

            curr.Left = entryNode;
            entryNode.Right = curr;

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
            ColumnType entryColumn = entryNode.Value.GetColumn();
            ColumnType currColumn;
            int comparisonResult;
            while (curr.Right != null)
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

                    prevNode.Right = entryNode;
                    entryNode.Left = prevNode;

                    nextNode.Left = entryNode;
                    entryNode.Right = nextNode;

                    return true;
                }
                curr = curr.Right;
            }
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
            RowType entryRow = entryNode.Value.GetRow();
            RowType currRow;
            int comparisonResult;
            while (curr.Down != null)
            {
                currRow = curr.Value.GetRow();
                comparisonResult = currRow.CompareTo(entryRow);
                if (comparisonResult == 0)
                {
                    return false;
                }
                else if (comparisonResult > 0)
                {
                    var prevNode = curr.Down;
                    var nextNode = curr;

                    prevNode.Down = entryNode;
                    entryNode.Up = prevNode;

                    nextNode.Down = entryNode;
                    entryNode.Up = nextNode;

                    return true;
                }
                curr = curr.Down;
            }
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

    }
}

using _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos
{
    /// <summary>
    /// Lista doble. No acepta nulos
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DoubleList<T> : IEnumerable<T> 
        where T : class
    {
        public int Length { private set;  get; }

        private DoubleNode<T> First;
        private DoubleNode<T> Last;

        public bool AddFirst(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (First == null)
            {
                First = new DoubleNode<T>(obj);
                Last = First;
                Length++;
                return true;
            }
            var previousFirst = First;
            var newFirst = new DoubleNode<T>(obj);
            previousFirst.Left = newFirst;
            newFirst.Right = previousFirst;
            First = newFirst;
            Length++;
            return true;
        }

        public bool AddLast(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (Last == null)
            {
                Last = new DoubleNode<T>(obj);
                First = Last;
                Length++;
                return true;
            }
            var previousLast = Last;
            var newLast = new DoubleNode<T>(obj);
            previousLast.Right = newLast;
            newLast.Left = previousLast;
            Last = newLast;
            Length++;
            return true;
        }

        public T GetFirst()
        {
            if (First == null)
                return null;
            return First.Value;
        }

        public T GetLast()
        {
            if (Last == null)
                return null;
            return Last.Value;
        }

        public bool Delete(T obj)
        {
            var curr = First;
            while (curr != null)
            {
                if (curr.Value.Equals(obj))
                {
                    DeleteNode(curr);
                    return true;
                }
                curr = curr.Right;
            }
            return false;
        }

        private void DeleteNode(DoubleNode<T> curr)
        {
            var left = curr.Left;
            var right = curr.Right;

            if (left == null)//curr es first
                First = right;
            else
                left.Right = right;

            if (right == null)//curr es last
                Last = left;
            else
                right.Left = left;
            Length--;
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= Length)
                return false;
            DoubleNode<T> curr = First;
            for (int i = 0; i < index + 1; i++)
            {
                curr = curr.Right;
            }
            DeleteNode(curr);
            return true;
        }

        /// <summary>
        /// retorna null ni no encuentra el elemento
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Get(int index)
        {
            if (index < 0 || index >= Length)
                return null;
            DoubleNode<T> curr = First;
            for (int i = 0; i < index; i++)
            {
                curr = curr.Right;
            }
            return curr.Value;
        }

        /// <summary>
        /// retorna el primer elemento en el que su .equals es verdadero
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T Get(T obj)
        {
            var curr = First;
            while (curr != null)
            {
                if (curr.Value.Equals(obj))
                {
                    return curr.Value;
                }
                curr = curr.Right;
            }
            return null;
        }

        //Chapuz medio o alto. Creo???. Estudiar mas enumerable
        private IEnumerable<T> GetMyEnumerable()
        {
            var curr = First;
            while (curr != null)
            {
                yield return curr.Value;
                curr = curr.Right;
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return GetMyEnumerable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)GetMyEnumerable()).GetEnumerator();
        }

        public string GetDot()
        {
            var sb = new StringBuilder();
            sb.Append("digraph Sparce_Matrix{\n");
            sb.Append("node[shape = box];\n");

            var curr = First;
            while (curr != null)
            {
                sb.Append(curr.GetHashCode() + "[label = \"" + curr.GetDotLabel() + "\"];\n");
                if (curr.Left != null)
                {
                    sb.Append(curr.GetHashCode() + "->" + curr.Left.GetHashCode() + ";\n");
                }
                if (curr.Right != null)
                {
                    sb.Append(curr.GetHashCode() + "->" + curr.Right.GetHashCode() + ";\n");
                }
                curr = curr.Right;
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}

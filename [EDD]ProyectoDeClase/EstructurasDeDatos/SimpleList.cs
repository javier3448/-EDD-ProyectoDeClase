using _EDD_ProyectoDeClase.EstructurasDeDatos.Nodos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos
{
    public class SimpleList<T> : IEnumerable<T>
        where T : class
    {
        public int Length { private set; get; }

        private SimpleNode<T> First;
        private SimpleNode<T> Last;

        public bool AddFirst(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (First == null)
            {
                First = new SimpleNode<T>(obj);
                Last = First;
                return true;
            }
            var previousFirst = First;
            var newFirst = new SimpleNode<T>(obj);
            newFirst.Next = previousFirst;
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
                Last = new SimpleNode<T>(obj);
                First = Last;
                return true;
            }
            var previousLast = Last;
            var newLast = new SimpleNode<T>(obj);
            previousLast.Next = newLast;
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
            if (First == null)
                return false;
            var currPrev = First;
            var curr = currPrev.Next;
            if (currPrev.Value.Equals(obj))
            {
                DeleteNode(null, currPrev);
                return true;
            }
            while (curr != null)
            {
                if (curr.Value.Equals(obj))
                {
                    DeleteNode(currPrev, curr);
                    return true;
                }
                curr = curr.Next;
                currPrev = currPrev.Next;
            }
            return false;
        }

        /// <summary>
        /// Elimina node y utilza prev para que sea mas facil. Chapuz bajo
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="node"></param>
        private void DeleteNode(SimpleNode<T> prev, SimpleNode<T> node)
        {
            var left = prev;
            var right = node.Next;

            if (left == null)//curr es first
                First = right;
            else
                left.Next = right;

            if (right == null)//curr es last
                Last = left;

            Length--;
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= Length)
                return false;
            if (First == null)
                return false;

            var currPrev = First;
            var curr = currPrev.Next;
            if (index == 0)
            {
                DeleteNode(null, currPrev);
                return true;
            }
            for (int i = 1; i < index + 1; i++)
            {
                curr = curr.Next;
                currPrev = currPrev.Next;
            }
            DeleteNode(currPrev, curr);
            Length--;
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
            SimpleNode<T> curr = First;
            for (int i = 0; i < index + 1; i++)
            {
                curr = curr.Next;
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
                curr = curr.Next;
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
                curr = curr.Next;
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
    }
}

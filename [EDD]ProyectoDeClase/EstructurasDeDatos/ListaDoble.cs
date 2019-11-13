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
    public class ListaDoble<T> where T : class
    {
        public int Length { private set;  get; }

        private NodoDoble<T> First;
        private NodoDoble<T> Last;

        public bool AddFirst(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (First == null)
            {
                First = new NodoDoble<T>(obj);
                Last = First;
            }
            var previousFirst = First;
            First = new NodoDoble<T>(obj);
            previousFirst.Left = First;
            First.Right = previousFirst;
            Length++;
            return true;
        }

        public bool AddLast(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            if (Last == null)
            {
                Last = new NodoDoble<T>(obj);
                First = Last;
            }
            var previousLast = Last;
            Last = new NodoDoble<T>(obj);
            previousLast.Right = Last;
            Last.Left = previousLast;
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

        private void DeleteNode(NodoDoble<T> curr)
        {
            var left = curr.Left;
            var right = curr.Right;

            curr.Left = null;
            curr.Right = null;

            left.Right = right;
            right.Left = left;
            Length--;
        }

        public bool Delete(int index)
        {
            if (index < 0 || index >= Length)
                return false;
            NodoDoble<T> curr = First;
            for (int i = 0; i < index + 1; i++)
            {
                curr = curr.Right;
            }
            DeleteNode(curr);
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
            NodoDoble<T> curr = First;
            for (int i = 0; i < index + 1; i++)
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

        public IEnumerator<T> MyEnumerator()
        {
            var curr = First;
            while (curr != null)
            {
                yield return curr.Value;
                curr = curr.Right;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EDD_ProyectoDeClase.EstructurasDeDatos
{
    public class MyHashtable<KeyType, ValueType> : IEnumerable<ValueType>
        where KeyType : class, IMyHash
        where ValueType : class
    {
        private DoubleList<MyPair>[] Array;
        //Numero de elementos en la lista
        public int Count { get; private set; } = 0;
        //Tamanno del array
        public int Size { get; private set; } = 0;

        private class MyPair
        {
            public KeyType Key;
            public ValueType Value;

            public MyPair(KeyType key, ValueType value)
            {
                Key = key;
                Value = value;
            }
        }

        public MyHashtable() : this(17) {   }

        /// <summary>
        /// Solo deberia de aceptar numero primos para que se puede usar mod para obtener un indice valido
        /// </summary>
        /// <param name="size"></param>
        public MyHashtable(int size)
        {
            if (size <= 0)
                throw new Exception("No puede tener una tabla con 0 o menos posiciones");
            Size = size;
            Array = new DoubleList<MyPair>[Size];
            for (int i = 0; i < Size; i++)
                Array[i] = new DoubleList<MyPair>();
        }

        public bool Add(KeyType key, ValueType value)
        {
            var hash = key.GetMyHash();
            var index = hash % Size;
            var list = Array[index];
            list.AddLast(new MyPair(key, value));//Chapuz agrega aunque no tenga key unica. 
            return true;
        }

        public ValueType Get(KeyType key)
        {
            var hash = key.GetMyHash();
            var index = hash % Size;
            var list = Array[index];
            foreach (var pair in list)
            {
                if (pair.Key.GetMyHash() == hash)
                    return pair.Value;
            }
            return null;
        }

        //Chapuz medio o alto. Creo???. Estudiar mas enumerable
        private IEnumerable<ValueType> GetMyEnumerable()
        {
            DoubleList<MyPair> list;
            for (int i = 0; i < Size; i++)
            {
                list = Array[i];
                foreach (var pair in list)
                {
                    yield return pair.Value;
                }
            }
        }
        public IEnumerator<ValueType> GetEnumerator()
        {
            return GetMyEnumerable().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)GetMyEnumerable()).GetEnumerator();
        }
    }
}

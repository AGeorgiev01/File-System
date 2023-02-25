using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opit4
{
    [Serializable]
    class HashTable
    {
        [Serializable]
        private class Node
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public Node Next { get; set; }

            public Node(string key, object value)
            {
                Key = key;
                Value = value;
            }
        }

        private Node[] table;

        public HashTable()
        {
            table = new Node[1024];
        }

        public void Add(string key, object value)
        {
            int index = GetIndex(key);
            Node node = table[index];

            while (node != null)
            {
                if (node.Key == key)
                {
                    node.Value = value;
                    return;
                }
                node = node.Next;
            }

            Node newNode = new Node(key, value);
            newNode.Next = table[index];
            table[index] = newNode;
        }

        public object Get(string key)
        {
            int index = GetIndex(key);
            Node node = table[index];

            while (node != null)
            {
                if (node.Key == key)
                {
                    return node.Value;
                }
                node = node.Next;
            }

            return null;
        }

        public bool ContainsKey(string key)
        {
            int index = GetIndex(key);
            Node node = table[index];

            while (node != null)
            {
                if (node.Key == key)
                {
                    return true;
                }
                node = node.Next;
            }

            return false;
        }

        public void Remove(string key)
        {
            int index = GetIndex(key);
            Node node = table[index];

            if (node == null)
            {
                return;
            }

            if (node.Key == key)
            {
                table[index] = node.Next;
                return;
            }

            while (node.Next != null)
            {
                if (node.Next.Key == key)
                {
                    node.Next = node.Next.Next;
                    return;
                }
                node = node.Next;
            }
        }

        private int GetIndex(string key)
        {
            int hash = 0;

            foreach (char c in key)
            {
                hash += c;
            }

            return hash % table.Length;
        }
        public void PrintTable()
        {
            for (int i = 0; i < table.Length; i++)
            {
                Node current = table[i];
                while (current != null)
                {
                    Console.WriteLine(current.Value.ToString());
                    current = current.Next;
                }
            }
        }
    }
}

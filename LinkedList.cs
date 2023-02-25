using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opit4
{
    [Serializable]
    class LinkedList
    {
        [Serializable]
        public class Node
        {
            public object Value { get; set; }
            public Node Next { get; set; }

            public Node(object value)
            {
                Value = value;
            }
        }

        private Node head;

        public void Add(object value)
        {
            Node node = new Node(value);
            node.Next = head;
            head = node;
        }

        public void Remove(object value)
        {
            if (head == null)
            {
                return;
            }

            if (head.Value == value)
            {
                head = head.Next;
                return;
            }

            Node node = head;
            while (node.Next != null)
            {
                if (node.Next.Value == value)
                {
                    node.Next = node.Next.Next;
                    return;
                }
                node = node.Next;
            }
        }

        public object Get(string directoryName)
        {
            Node node = head;
            while (node != null)
            {
                DirectoryNode directoryNode = (DirectoryNode)node.Value;
                if (directoryNode.name == directoryName)
                {
                    return directoryNode;
                }
                node = node.Next;
            }

            return null;
        }

        public bool Contains(string directoryName)
        {
            Node node = head;
            while (node != null)
            {
                DirectoryNode directoryNode = (DirectoryNode)node.Value;
                if (directoryNode.name == directoryName)
                {
                    return true;
                }
                node = node.Next;
            }

            return false;
        }
        public void PrintList()
        {
            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.Value.ToString());
                current = current.Next;
            }
        }
    }
}

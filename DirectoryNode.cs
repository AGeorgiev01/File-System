using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opit4
{
    [Serializable]
    class DirectoryNode
    {
        private LinkedList subDirectories; //subdir llist
        private HashTable files; //htable for files
        public string name; //dirname
        public DirectoryNode parent { get; set; }


        public DirectoryNode(string name)
        {
            this.name = name;
            this.subDirectories = new LinkedList(); //int new llist for subdir
            this.files = new HashTable(); //new htable for files
        }

 public void AddFile(string fileName, byte[] content, int blockSize, bool isAppend = false)
    {
            //af with 3 inputs
        File file = new File(fileName, content, blockSize, isAppend);
        files.Add(fileName, file);
    }

        public void RemoveFile(string fileName)
        {
            //rf
            files.Remove(fileName);
        }

        public void WriteFile(string fileName, string content, bool isAppend = false)
        {
            if (!files.ContainsKey(fileName))
            {
                Console.WriteLine("Error: file not found.");
                return;
            }
            //rewrite
            File file = (File)files.Get(fileName);
            file.Write(content, isAppend);
        }

        public void AddDirectory(DirectoryNode directory)
        {
            subDirectories.Add(directory);
        }

        public void RemoveDirectory(DirectoryNode directory)
        {
            subDirectories.Remove(directory);
        }

        public DirectoryNode GetDirectory(string directoryName)
        {
            if (!subDirectories.Contains(directoryName))
            {
                Console.WriteLine("Error: directory not found.");
                return null;
            }
            return (DirectoryNode)subDirectories.Get(directoryName);
        }

        public LinkedList GetSubDirectories()
        {
            return subDirectories;
        }

        public HashTable GetFiles()
        {
            return files;
        }

        public string GetFullPath()
        {
            DirectoryNode currentNode = this;
            string fullPath = currentNode.name;

            while (currentNode.parent != null)
            {
                currentNode = currentNode.parent;
                fullPath = currentNode.name + "/" + fullPath;
            }
            return "/" + fullPath;
        }
    }
}
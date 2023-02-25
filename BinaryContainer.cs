using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace opit4
{
    [Serializable]
    class BinaryContainer
    {
        private DirectoryNode rootDirectory;
        private string containerPath;
        private string path;
        public DirectoryNode GetRootDirectory()
        {
            return rootDirectory;
        }
        public BinaryContainer()
        {
            rootDirectory = new DirectoryNode("root");
        }
        public BinaryContainer(string containerPath)
        {
            path = containerPath;
            this.containerPath = containerPath;
            if (System.IO.File.Exists(containerPath))
            {
                LoadFromFile();
            }
            else
            {
                rootDirectory = new DirectoryNode("root");
            }
        }
        public void SaveToFile(string filePath)
        {
            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            // Create a new file
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                // Serialize the FileSystem object to the file
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this.rootDirectory);
            }
            Console.WriteLine("File saved at: " + filePath);
        }

        public void LoadFromFile()
        {
            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                Console.WriteLine("No files");
            }
            // Check if file is empty
            else if (new System.IO.FileInfo(path).Length == 0)
            {
                Console.WriteLine("The file is empty.");
            }
            // Open the file and deserialize the FileSystem object
            else
            {
                using (FileStream fs = System.IO.File.OpenRead(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    rootDirectory = (DirectoryNode)formatter.Deserialize(fs);
                }
            }
        }
    }
}

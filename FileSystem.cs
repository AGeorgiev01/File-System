using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace opit4
{
    [Serializable]
    class FileSystem
    {
        private string containerPath;
        private int blockSize;
        private Deduplication deduplication;
        private DirectoryNode currentDirectory;

        public FileSystem(int blockSize)
        {
            // Initblock size and create a new instance of Dedupl.
            this.blockSize = blockSize;
            deduplication = new Deduplication(blockSize);
            //new root dir.
            currentDirectory = new DirectoryNode(null);
        }

        public void CreateDirectory(string directoryName)
        {
            //mkdir
            currentDirectory.AddDirectory(new DirectoryNode(directoryName));
        }

        public void DeleteDirectory(string directoryName)
        {
            //rmdir
            DirectoryNode directory = currentDirectory.GetDirectory(directoryName);
            if (directory != null)
            {
                currentDirectory.RemoveDirectory(directory);
            }
        }

        public void ListContents()
        {
            //subdirs
            LinkedList subDirectories = currentDirectory.GetSubDirectories();
            HashTable files = currentDirectory.GetFiles();
            //cw names ??
            Console.WriteLine("Directories:");
            subDirectories.PrintList();
            Console.WriteLine("Files:");
            files.PrintTable();
        }

        public void ChangeDirectory(string directoryName)
        {
            //set to curr
            DirectoryNode directory = currentDirectory.GetDirectory(directoryName);
            if (directory != null)
            {
                currentDirectory = directory;
            }
        }

        public void CopyFile(string sourceFile, string destinationFile)
        {
            File file = (File)currentDirectory.GetFiles().Get(sourceFile);
            if (file != null)
            {
                byte[] content = file.GetContent();
                int blockSize = file.BlockSize;
                currentDirectory.AddFile(destinationFile, content, blockSize);
            }
        }

        public void DeleteFile(string fileName)
        {
            File file = (File)currentDirectory.GetFiles().Get(fileName);
            deduplication.RemoveFile(file);
            currentDirectory.RemoveFile(fileName);
        }

        public void DisplayFile(string fileName)
        {
            //cat
            File file = (File)currentDirectory.GetFiles().Get(fileName);
            if (file != null)
            {
                //show if exists
                Console.WriteLine(Encoding.ASCII.GetString(file.GetContent()));
            }
        }
        public void WriteFile(string fileName, string content, bool isAppend = false)
        {
            File file = (File)currentDirectory.GetFiles().Get(fileName);
            if (file != null)
            {
                //string to byte arr
                byte[] newContent = Encoding.ASCII.GetBytes(content);
                //new content
                file.SetContent(newContent);
                //remove from dedupl
                deduplication.RemoveFile(file);
                //add back
                deduplication.AddFile(file);
                //update IsApp
                file.IsAppend = isAppend;
            }
        }
        public void CreateFile(string fileName, byte[] content, int blockSize, bool isAppend = false)
        {
            File file = new File(fileName, content, blockSize, isAppend);
            //add to dedupl
            deduplication.AddFile(file);
            currentDirectory.AddFile(fileName, content, blockSize);
        }



        public DirectoryNode GetCurrentDirectory()
        {
            return currentDirectory;
        }

        public void SaveToFile(string filePath)
        {
            //save to fPath
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, currentDirectory);
            }
            Console.WriteLine("File system saved at: " + filePath);
        }
        public void LoadFromFile(string containerPath)
        {
            this.containerPath = containerPath;
            using (FileStream stream = new FileStream(containerPath, FileMode.Open))
            {
                //load from contPath
                BinaryFormatter formatter = new BinaryFormatter();
                DirectoryNode root = (DirectoryNode)formatter.Deserialize(stream);
                this.currentDirectory = root;
                deduplication = new Deduplication(blockSize);
            }
        }
    }
}

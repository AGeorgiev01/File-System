using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace opit4
{
    [Serializable]
    class Program
    {
        static void Main(string[] args)
        {
            int blockSize = 0;
            Console.Write("Enter block size:");
            blockSize = int.Parse(Console.ReadLine());
            FileSystem fileSystem = new FileSystem(blockSize);

            Console.Write("Import container path or press enter to skip: ");
            string containerPath = Console.ReadLine();
            if (!string.IsNullOrEmpty(containerPath) && System.IO.File.Exists(containerPath))
            {
                fileSystem.LoadFromFile(containerPath);
            }

            Console.WriteLine("Possible commands: mkdir, ls, cd, rmdir, , write, +app, cat, rm, export, exit");
                while (true)
                {
                    Console.Write("Enter command: ");
                string input = Console.ReadLine();
                string[] inputArr = input.Split(' ');
                if (inputArr.Length == 0) continue;
                string command = inputArr[0];

                string[] commandArgs = new string[inputArr.Length - 1];
                Array.Copy(inputArr, 1, commandArgs, 0, inputArr.Length - 1);

                switch (command)
                    {
                        case "mkdir":
                            fileSystem.CreateDirectory(commandArgs[0]);
                            Console.WriteLine("Directory " + commandArgs[0] + " created.");
                            break;
                        case "rmdir":
                            fileSystem.DeleteDirectory(commandArgs[0]);
                            Console.WriteLine("Directory " + commandArgs[0] + " deleted.");
                            break;
                        case "ls":
                            fileSystem.ListContents();
                            break;
                        case "cd":
                            fileSystem.ChangeDirectory(commandArgs[0]);
                            Console.WriteLine("Current directory changed to " + fileSystem.GetCurrentDirectory().GetFullPath());
                            break;
                        /*case "cp":
                            fileSystem.CopyFile(commandArgs[0], commandArgs[1]);
                            Console.WriteLine("File " + commandArgs[0] + " copied to " + commandArgs[1]);
                            break;*/
                        case "rm":
                            fileSystem.DeleteFile(commandArgs[0]);
                            Console.WriteLine("File " + commandArgs[0] + " deleted.");
                            break;
                        case "cat":
                            fileSystem.DisplayFile(commandArgs[0]);
                            break;
                        case "+app":
                            bool isAppend = false;
                            if (commandArgs.Length > 2 && commandArgs[2] == "+app")
                                isAppend = true;
                            fileSystem.WriteFile(commandArgs[0], commandArgs[1], isAppend);
                            if (isAppend)
                                Console.WriteLine("Content added to file " + commandArgs[0]);
                            else
                                Console.WriteLine("Content written to file " + commandArgs[0]);
                            break;
                        case "write":
                            if (commandArgs.Length < 2)
                            {
                                Console.WriteLine("Error: file name or content not provided.");
                                return;
                            }
                            bool appendMode = false;
                            if (commandArgs.Length == 3 && commandArgs[2].Equals("+app", StringComparison.OrdinalIgnoreCase))
                                appendMode = true;
                            else if (commandArgs.Length == 3)
                            {
                                blockSize = int.Parse(commandArgs[2]);
                            }
                            byte[] content = Encoding.ASCII.GetBytes(commandArgs[1]);
                            fileSystem.CreateFile(commandArgs[0], content, blockSize, appendMode);
                            if (appendMode)
                                Console.WriteLine("Content added to file " + commandArgs[0]);
                            else
                                Console.WriteLine("File " + commandArgs[0] + " created.");
                            break;
                        case "export":
                            if (commandArgs.Length == 0)
                            {
                                Console.WriteLine("Error: Please provide a file path.");
                                break;
                            }
                            fileSystem.SaveToFile(commandArgs[0]);
                            break;
                        default:
                            Console.WriteLine("Error: Invalid command.");
                            break;
                        case "exit":
                            Console.WriteLine("Exiting the program...");
                            return;
                    }
                    Console.WriteLine("Current directory: " + fileSystem.GetCurrentDirectory().GetFullPath());
                }
            }
        }
    }

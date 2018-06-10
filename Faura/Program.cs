﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Faura
{
    class Program
    {
        static void Main(string[] args)
        {
            /*using (FileStream strm = new FileStream(@"D:\Ar Tonelico\portrait_test\portrait_test.txt", FileMode.Open, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(strm);
                writer.AutoFlush = true;

                writer.WriteLine("# version metafalica");
                writer.WriteLine();

                for (int i = 0; i < 500; i++)
                {
                    writer.WriteLine("DisplayDialogSeries 0 0");
                    writer.WriteLine("ClearPortrait 0 -10");
                    writer.WriteLine($"SetPortrait 0 { i }");
                }
            }*/

            /*using (FileStream strm = new FileStream(@"D:\Ar Tonelico\portrait_test\portrait_test_msg.txt", FileMode.Open, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(strm);
                writer.AutoFlush = true;

                writer.WriteLine("[");

                for (int i = 100; i < 200; i++)
                {
                    writer.WriteLine("\t{");

                    writer.WriteLine("\t\t\"Name\": \"0\",");
                    writer.WriteLine("\t\t\"TextboxType\": \"FullPortrait\",");
                    writer.WriteLine("\t\t\"CharacterName\": \"Luca\",");
                    writer.WriteLine($"\t\t\"CharacterID\": \"{ i }\",");
                    writer.WriteLine($"\t\t\"PortraitPosition\": \"{ Faura.Enums.Metafalica.PortraitPosition.LEFT_CONVERSATION }\",");
                    writer.WriteLine($"\t\t\"Text\": \"{ i }\"");

                    writer.WriteLine("\t},");
                }

                writer.WriteLine("]");
            }*/

            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                ShowHelp();
                return;
            }

            string[] processedArgs = ProcessArgs(args);
            Event ev = new Event(processedArgs[0], processedArgs[1], processedArgs[2]);
        }

        private static string[] ProcessArgs(string[] args)
        {
            string[] procArgs = new string[] { "", "", "" };

            for (int i = 0; i < args.Length; i++)
            {
                if (i + 1 >= args.Length)
                    throw new Exception("The parameters were malformed.");

                switch(args[i])
                {
                    case "-i":
                    case "--input":
                        procArgs[0] = args[i + 1];
                        break;
                    case "-o":
                    case "--output":
                        procArgs[1] = args[i + 1];
                        break;
                    case "-v":
                    case "--version":
                        procArgs[2] = args[i + 1];
                        break;
                    default:
                        throw new Exception($"Unknown parameter \"{ args[i] }\"");
                }

                // Increment the counter to skip to the next parameter
                i++;
            }

            return procArgs;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Faura: A compiler/decompiler for the event files from some of Gust's PS2 JRPGs.");
            Console.WriteLine("Written by Gamma/@SageOfMirrors.");
            Console.WriteLine("For any issues or questions, visit the GitHub repo at\nhttps://github.com/Sage-of-Mirrors/Faura\n");
            Console.WriteLine("Usage: Faura.exe --input filePath --version version [--output filePath]");
            Console.WriteLine();
            Console.WriteLine("Parameters:");
            Console.WriteLine("\t--input/-i   filePath\tThe input file, either a .evd or a .txt containing event data");
            Console.WriteLine("\t--output/-o  filePath\tThe output file for a .evd, or the output directory for .txt files");
            Console.WriteLine("\t--version/-v version\tThe source game of the .evd, or the target game for .txt files");
            Console.WriteLine("\t--help/-h\t\tDisplay this help message");
            Console.WriteLine();
            Console.WriteLine("Supported Versions:");
            Console.WriteLine("\tmetafalica: Ar tonelico II: Melody of Metafalica");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faura
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] processedArgs = ProcessArgs(args);
            Event ev = new Event(processedArgs[0], processedArgs[1], processedArgs[2]);
            //ShowHelp();
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
            Console.WriteLine("Faura: A compiler/decompiler for Ar Tonelico and Ar Tonelico 2's event files.");
            Console.WriteLine("Written by Gamma/@SageOfMirrors.");
            Console.WriteLine("For any issues or questions, visit the GitHub repo at\nhttps://github.com/Sage-of-Mirrors/Faura\n");
            Console.WriteLine("Usage: Faura.exe -input *.evd/*.txt [-output path]");
        }
    }
}

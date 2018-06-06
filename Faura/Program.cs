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
            Event ev = new Event(args[0]);
            //ShowHelp();
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

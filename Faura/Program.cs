using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faura.src;
using System.IO;
using Faura.src.Parsing.Elemia;

namespace Faura
{
    class Program
    {
        static void Main(string[] args)
        {
            ElemiaEventParser parser = new ElemiaEventParser();
            parser.ParseRawData(@"D:\Ar Tonelico\gust-tools-v.0.7.1.1-x64\Elemia_RPK\Event\1.evd");
            //Event ev = new Event(args[0]);
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

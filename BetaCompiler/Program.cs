using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace BetaCompiler {
    class Program {
        static void Main(string[] args)
        {
            var lexer = new Lexer.Lexer(args[0]);
            var l = lexer.Parse();
            l.Add("$");
            var parser = new Parser(l);
            parser.Parse();

            Console.ReadLine();
        }
    }
}

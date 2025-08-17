using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Untlited_Programming_Game
{
    internal static class Program
    {
        public static void Main()
        {
            string text = File.ReadAllText(@"C:\Users\lucas\Downloads\Lucas\Codigo.txt");
            Processor processor = new Processor(new[] { ("A", true, true), ("B", true, true) });
            Parser.Parser parser = new Parser.Parser();
            Instructions.Instruction[] instructions = parser.parseProgram(text);
            processor.loadProgram(instructions);
            bool running = true;
            processor.onEnd += () =>
            {
                running = false;
            };
            while (running)
            {
                processor.Execute();
                Console.WriteLine(processor.getCurrentLine());
            }


        }
    }
}

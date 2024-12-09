// See https://aka.ms/new-console-template for more information
using Untlited_Programming_Game;
using Untlited_Programming_Game.Instructions;
using Untlited_Programming_Game.Parser;
using Untlited_Programming_Game.Exceptions;

Processor processor = new Processor();
List<string> instructions = new List<string>();

string readedInstruction;
int line = 0;
Console.WriteLine("Insira as instruções");
do
{
    Console.Write(line + " - ");
    readedInstruction = Console.ReadLine();
    if (readedInstruction != "end")
    {
        instructions.Add(readedInstruction);
        line++;
    }
} while (readedInstruction != "end");
CodeException[] errors;
processor.loadProgram(Parser.parseProgram(instructions.ToArray(), out errors));
foreach (CodeException error in errors)
{
    Console.WriteLine(error.Message + " - " + error.line);
}
if (errors.Length == 0)
{
    Console.WriteLine("\nExecutando código:");
    processor.RunTillFinish();
    processor.Print();
}
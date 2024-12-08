// See https://aka.ms/new-console-template for more information
using Untlited_Programming_Game;
using Untlited_Programming_Game.Instructions;
using Untlited_Programming_Game.Parser;

Processor processor = new Processor();
List<Instruction> instructions = new List<Instruction>();
string readedInstruction;
Console.WriteLine("Insira as instruções");
do
{
    readedInstruction = Console.ReadLine();
    if (readedInstruction != "end")
    {
        instructions.Add(Parser.parseInstruction(readedInstruction));
    }
} while (readedInstruction != "end");
processor.loadProgram(instructions.ToArray());
processor.RunTillFinish();
processor.Print();
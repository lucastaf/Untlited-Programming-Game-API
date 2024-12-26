// See https://aka.ms/new-console-template for more information
//using Untlited_Programming_Game;
//using Untlited_Programming_Game.Instructions;
//using Untlited_Programming_Game.Parser;
//using Untlited_Programming_Game.Exceptions;

//Processor processor = new Processor();

//string readedInstruction = File.ReadAllText(@"C:\Users\lucas\Downloads\Codigo.txt");
//CodeException[] errors;
//Instruction[] instructions = Parser.parseProgram(readedInstruction, out errors);
//processor.loadProgram(instructions);
//foreach (CodeException error in errors)
//{
//    Console.WriteLine(error.Message + " - " + error.line);
//}
//if (errors.Length == 0)
//{
//    Console.WriteLine("\nExecutando código:");
//    processor.RunTillFinish();
//    processor.Print();
//}
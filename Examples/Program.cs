using System;
using Functional.Solutions._02Option;

namespace Examples
{
  public class Program
  {
    public static void Main()
    {
      Console.Write("Which scenario do we want to run?\n1. Option\n2. Result\n3. Async Result\nAnswer: ");
      Console.ReadLine()
             .TryParseInt()
             .Bind(Run)
             .Match(
               i => Console.WriteLine("Successfully ran scenario {0}!", i),
               () => Console.WriteLine("Invalid scenario!"));

      Console.WriteLine("Finished scenario run. Press any key to exit...");
      Console.ReadKey();
    }

    private static Option<int> Run(int choice)
    {
      switch (choice)
      {
        case 1:
          OptionExample.Run();
          return choice;
        case 2:
          ResultExample.Run();
          return choice;
        case 3:
          AsyncResultExample.Run().Wait();
          return choice;
        default:
          return Option.None;
      }
    }
  }
}

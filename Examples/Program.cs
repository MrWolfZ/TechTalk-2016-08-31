using System;
using Functional.Solutions._02Option;

namespace Examples
{
  public class Program
  {
    public static void Main()
    {
      Console.Write("Which scenario do we want to run?\n1. Option\n2. Result\nAnswer: ");
      Console.ReadLine()
             .TryParseInt()
             .Bind(Run)
             .Match(
               i => Console.WriteLine("Successfully ran scenario {0}!", i),
               () => Console.WriteLine("An error occured!"));

      Console.WriteLine("Finished scenario run. Press any key to exit...");
      Console.ReadKey();
    }

    private static Option<int> Run(int choice)
    {
      switch (choice)
      {
        case 1:
          OptionExample.Run();
          return 1;
        default:
          return Option.None;
      }
    }
  }
}

using System;

class Program
{
    static void Main()
    {
        // Change text color to green
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("This text is green.");
        Console.WriteLine("Hello World!");

        // Change text color to red
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("This text is red.");

        // Reset to default colors
        Console.ResetColor();
        Console.WriteLine("This text is the default color.");

        // Set background color to blue
        Console.BackgroundColor = ConsoleColor.Blue;
        // Set text color to white
        Console.ForegroundColor = ConsoleColor.White;
        
        // Print some text with the new colors
        Console.WriteLine("This text has a blue background and white text.");
        
        // Reset colors to default
        Console.ResetColor();
        Console.WriteLine("This text is using default colors.");
    }
}
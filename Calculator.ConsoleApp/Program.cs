using System;

namespace Calculator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your expression: ");

            var expression = Console.ReadLine();

            Console.WriteLine(Common.Calculator.Calculate(expression));
            Console.ReadLine();
        }        
    }
}
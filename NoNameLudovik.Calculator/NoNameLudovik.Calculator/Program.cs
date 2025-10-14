using System.Text.RegularExpressions;
using CalculatorLibrary;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;

        Calculator calculator = new Calculator();
        while (!endApp)
        {
            // Display title as the C# console calculator app.
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");
            // Declare variables and set to empty.
            double result = 0;
            string? op;

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tp - Power");
            Console.WriteLine("\tsq - Square Root");
            Console.WriteLine("\tsin - Sinus");
            Console.WriteLine("\tcos - Cosinus");
            Console.WriteLine("\ttan - Tangent");
            Console.WriteLine("\th - Show History of Operation");
            Console.Write("Your option? ");

            // Validate input is not null, and matches the pattern
            while (true)
            {
                op = Console.ReadLine();

                if (op == null || !Regex.IsMatch(op, "[a|s|m|d|h|p|sin|cos|tan|cot]"))
                {
                    Console.Write("Error: Unrecognized input. Try again:");
                    continue;
                }
                break;
            }

            if (op == "h")
            {
                calculator.ShowHistory();
                continue;
            }

            double firstNumber;
            double secondNumber;

            if (!Regex.IsMatch(op, "[sin|cos|tan]")) 
            {
                Console.Clear();
                Console.WriteLine("First Number");
                firstNumber = calculator.AskNumber();
                Console.WriteLine("Second Number");
                secondNumber = calculator.AskNumber();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Your Number");
                firstNumber = calculator.AskNumber();
                secondNumber = 0;
            }

            try
            {
                result = calculator.DoOperation(firstNumber, secondNumber, op);
                if (double.IsNaN(result))
                {
                    Console.WriteLine("This operation will result in a mathematical error.\n");
                }
                else Console.WriteLine("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }
            
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;

            Console.WriteLine("\n"); // Friendly linespacing.
            Console.Clear();
        }
        // Add call to close the JSON writer before return
        calculator.Finish();
        return;
    }
}
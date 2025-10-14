using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        List<Calculation> history = new List<Calculation>(); //List for history of calculations

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    op = "Add";
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    op = "Subtract";
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    op = "Multiply";
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    op = "Divide";
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            history.Add(new Calculation(num1, num2, op, result));//Add operation to history

            return result;
        }

        //Method for showing history to user
        public void ShowHistory() 
        {
            Console.Clear();
            //Check if list is empty. If not than show to user all objects from calculations List
            if (history.Count == 0)
            {
                Console.WriteLine("History is empty!)");
                Console.ReadLine();
            }
            else
            {
                int index = 1;
                foreach (Calculation calculation in history)
                {
                    Console.WriteLine($"{index}. Numbers:{calculation.num1}, {calculation.num2}|{calculation.op}|Result: {calculation.result}");
                    index++;
                }

                Console.WriteLine("Type 'd' if you want to delete history or just press enter if you want to leave");
                if(Console.ReadLine() == "d")
                {
                    history.Clear();
                }
            }
            Console.Clear();
        }

        public double AskNumber()
        {
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput = "";
            // Ask the user to type the first number.
            Console.Write("Type a number or type 'h' if you want to choose result from history, and then press Enter: ");
            numInput = Console.ReadLine();

            double cleanNum = 0;
            while (!double.TryParse(numInput, out cleanNum))
            {
                if (numInput == "h" && history.Count() > 0)
                {

                    cleanNum = ChooseResultFromHistory();
                    break;
                }
                else if (numInput == "h" && history.Count() <= 0)
                {
                    Console.WriteLine("History is empty!) Type numeric value: ");
                    numInput = Console.ReadLine();
                }
                else
                {
                    Console.Write("This is not valid input. Please enter a numeric value: ");
                    numInput = Console.ReadLine();
                }
            }

            return cleanNum;
        }

        double ChooseResultFromHistory()
        {
            string? indexInput = " ";
            
            int index = 1;
            foreach (Calculation calculation in history)
            {
                Console.WriteLine($"{index}. Numbers:{calculation.num1}, {calculation.num2}|{calculation.op}|Result: {calculation.result}");
                index++;
            }

            Console.WriteLine("Type index of operation to choose result of it:");
            indexInput = Console.ReadLine();

            while (!Int32.TryParse(indexInput, out index))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                indexInput = Console.ReadLine();
            }

            index--;
            return history[index].result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }

    //Class for determine calculation objects in history
    public class Calculation
    {
        public double num1;
        public double num2;
        public string op;
        public double result;

        public Calculation(double num1, double num2, string op, double result)
        {
            this.num1 = num1;
            this.num2 = num2;
            this.op = op;
            this.result = result;
        }
    }
}
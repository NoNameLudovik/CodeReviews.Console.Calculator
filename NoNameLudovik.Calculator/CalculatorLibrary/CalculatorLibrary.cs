using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        List<Calculation> calculations = new List<Calculation>(); //List for history of calculations

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

            calculations.Add(new Calculation(num1, num2, op, result));//Add operation to history

            return result;
        }

        //Method for showing history to user
        public void ShowHistory() 
        {
            Console.Clear();
            //Check if list is empty. If not than show to user all objects from calculations List
            if (calculations.Count == 0)
            {
                Console.WriteLine("History is empty!)");
                Console.ReadLine();
            }
            else
            {
                int index = 1;
                foreach (Calculation calculation in calculations)
                {
                    Console.WriteLine($"{index}. Numbers:{calculation.num1}, {calculation.num2}|{calculation.op}|Result: {calculation.result}");
                    index++;
                }

                Console.WriteLine("Type 'd' if you want to delete history or just press enter if you want to leave");
                if(Console.ReadLine() == "d")
                {
                    calculations.Clear();
                }
            }
            Console.Clear();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7.CalculateExpression
{
    class Program
    {
        // Метод за определяне на приоритета
        static int GetPrecedence(char oper)
        {
            switch (oper)
            {
                case 's':
                case 'p':
                case 'l':
                    return 3;
                case '*':
                case '/':
                    return 2;
                case '+':
                case '-':
                    return 1;
                default:
                    break;
            }
            return 0;
        }

        // Метод за извършване на изчисленията
        static double CalculateExpression(char oper, Stack<double> exp)
        {
            double expression = 0;
            double lastNumber = exp.Pop();
            switch (oper)
            {
                case 's': expression = Math.Sqrt(lastNumber); // изчислява корен квадратен
                    break;
                case 'p': expression = Math.Pow(exp.Pop(), lastNumber); // изчислява повдигане на степен
                    break;
                case 'l': expression = Math.Log(lastNumber); // изчислява логаритъм
                    break;
                case '*': expression = exp.Pop() * lastNumber; // умножение
                    break;
                case '/': expression = exp.Pop() / lastNumber; // деление
                    break;
                case '+': expression = exp.Pop() + lastNumber; // събиране
                    break;
                case '-': expression = exp.Pop() - lastNumber; // изваждане
                    break;
                default:
                    break;
            }
            return expression;
        }

        static Queue<char> ShuntingYard(string input) // Използва се алгоритъма Shunting Yard, като в опашка се вкарват числата и операторите
        {
            Queue<char> output = new Queue<char>(); // опашка
            Stack<char> stack = new Stack<char>(); // стек за операторите
            for (int i = 0; i < input.Length; i++)
            {
                char symbol = input[i];
                if (symbol == '.' || symbol == ',' || Char.IsDigit(symbol))
                {
                    output.Enqueue(symbol);
                }
                else
                {
                    int preced = GetPrecedence(symbol);
                    if (preced > 0)
                    {
                        if (stack.Count == 0 || GetPrecedence(stack.Peek()) < preced)
                        {
                            stack.Push(symbol);
                            output.Enqueue(' ');
                        }
                        else
                        {
                            while (stack.Count > 0 && preced <= GetPrecedence(stack.Peek()))
                            {
                                output.Enqueue(stack.Pop());
                            }
                            stack.Push(symbol);
                        }
                    }
                    else
                    {
                        if (symbol == '(')
                        {
                            stack.Push(symbol);
                        }
                        else if (symbol == ')')
                        {
                            while (stack.Peek() != '(')
                            {
                                output.Enqueue(stack.Pop());
                            }
                            stack.Pop();
                            if (stack.Count > 0 && Char.IsLetter(stack.Peek()))
                            {
                                output.Enqueue(stack.Pop());
                            }
                        }
                    }
                }
            }
            while (stack.Count > 0)
            {
                output.Enqueue(stack.Pop());
            }
            return output;
        }

        static Stack<double> PolishNotation(Queue<char> output) // Използва се алгоритъма Polish Notation, за да се извърши изчислението в готовата опашка
        {
            Stack<double> exp = new Stack<double>();
            while (output.Count > 0)
            {
                char symbol = output.Dequeue();
                if (Char.IsDigit(symbol))
                {
                    string num = "";
                    while (Char.IsDigit(symbol) || symbol == '.')
                    {
                        num += symbol;
                        symbol = output.Dequeue();
                    }
                    double number = double.Parse(num);
                    exp.Push(number);
                }
                if (symbol != ' ' && symbol != ',')
                {
                    exp.Push(CalculateExpression(symbol, exp));
                }
               
            }
            return exp;
        }

        static void Main(string[] args)
        {
            string input = "(3+5.3) * 2.7 - ln(22) / pow(2.2, -1.7)";
            input = input.Replace(" ", "").Replace("(-", "(0-").Replace(",-", ",0-"); // добавя се 0, за да се избегнем проблема с отрицателните числа 
            Queue<char> output = new Queue<char>();
            output = ShuntingYard(input);
            double result = PolishNotation(output).Pop();
            Console.WriteLine(result);
        }
    }
}

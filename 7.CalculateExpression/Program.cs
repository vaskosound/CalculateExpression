using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7.CalculateExpression
{
    class Program
    {
        //  Determining the precedence
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

        static double CalculateExpression(char oper, Stack<double> exp)
        {
            double expression = 0;
            double lastNumber = exp.Pop();
            switch (oper)
            {
                case 's': expression = Math.Sqrt(lastNumber); 
                    break;
                case 'p': expression = Math.Pow(exp.Pop(), lastNumber);
                    break;
                case 'l': expression = Math.Log(lastNumber);
                    break;
                case '*': expression = exp.Pop() * lastNumber;
                    break;
                case '/': expression = exp.Pop() / lastNumber;
                    break;
                case '+': expression = exp.Pop() + lastNumber;
                    break;
                case '-': expression = exp.Pop() - lastNumber;
                    break;
                default:
                    break;
            }
            return expression;
        }

        // Shunting Yard algorithm
        static Queue<char> ShuntingYard(string input) 
        {
            Queue<char> output = new Queue<char>();
            Stack<char> operatorsStack = new Stack<char>();
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
                        if (operatorsStack.Count == 0 || GetPrecedence(operatorsStack.Peek()) < preced)
                        {
                            operatorsStack.Push(symbol);
                            output.Enqueue(' ');
                        }
                        else
                        {
                            while (operatorsStack.Count > 0 && preced <= GetPrecedence(operatorsStack.Peek()))
                            {
                                output.Enqueue(operatorsStack.Pop());
                            }
                            operatorsStack.Push(symbol);
                        }
                    }
                    else
                    {
                        if (symbol == '(')
                        {
                            operatorsStack.Push(symbol);
                        }
                        else if (symbol == ')')
                        {
                            while (operatorsStack.Peek() != '(')
                            {
                                output.Enqueue(operatorsStack.Pop());
                            }
                            operatorsStack.Pop();
                            if (operatorsStack.Count > 0 && Char.IsLetter(operatorsStack.Peek()))
                            {
                                output.Enqueue(operatorsStack.Pop());
                            }
                        }
                    }
                }
            }
            while (operatorsStack.Count > 0)
            {
                output.Enqueue(operatorsStack.Pop());
            }
            return output;
        }

        // Polish Notation algorithm - calculating expression in the output queue
        static Stack<double> PolishNotation(Queue<char> output)
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
            // add 0 to avoid the problem of negative numbers 
            input = input.Replace(" ", "").Replace("(-", "(0-").Replace(",-", ",0-"); 
            Queue<char> output = new Queue<char>();
            output = ShuntingYard(input);
            double result = PolishNotation(output).Pop();
            Console.WriteLine(result);
        }
    }
}

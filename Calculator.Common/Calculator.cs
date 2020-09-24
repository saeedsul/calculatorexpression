using System;
using System.Text.RegularExpressions;

namespace Calculator.Common
{
    public static class Calculator
    {
        private static string multiplyDividePattern = @"(?<firstNumber>[-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?)))(?<symbol>[*/])(?<secondNumber>[-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?)))";
        //@"(?<firstNumber>(\d+)|(\d+\.\d+))(?<symbol>[*\/])(?<secondNumber>(\d+)|(\d+\.\d+))";
        private static string addSubtractPatten = @"(?<firstNumber>(?<![*/\d-])([-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?))))(?<symbol>[-+])(?<secondNumber>[-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?))(?![/*\d]))";
        //@"(?<firstNumber>(\d+)|(\d+\.\d+))(?<symbol>[-+])(?<secondNumber>(\d+)|(\d+\.\d+))";

        private static string decimalsPattern = @"^\d+\.?\d+$";
        private static string allowedPattern = @"^\s*(\d+)(?:\s*([-+*\/])\s*(?:\d+)\s*)+$";

        public static string Calculate(string expression)
        {
            Regex regex = new Regex(allowedPattern);
            if(!regex.IsMatch(expression))
            {
                return "Invalid expression";
            }

            expression = expression.Replace(" ", "");

            double result;
            while (!double.TryParse(expression, out result))
            {
                expression = Compute(expression, multiplyDividePattern);

                expression = Compute(expression, addSubtractPatten);
            }
           
            return DoubleToFraction(decimalsPattern, result);          
        }

        private static string Compute(string expression, string pattern)
        {    
            Regex regex = new Regex(pattern);

            while (regex.IsMatch(expression))
            {
                Match match = regex.Match(expression);
                double firstNumber = double.Parse(match.Groups["firstNumber"].Value);
                double secondNumber = double.Parse(match.Groups["secondNumber"].Value);

                switch (match.Groups["symbol"].Value)
                {
                    case "-":
                        {
                            expression = expression.Remove(match.Index, match.Length);
                            expression = expression.Insert(match.Index, Subtract(firstNumber, secondNumber).ToString("0.##"));
                        }
                        break;
                    case "+":
                        {
                            expression = expression.Remove(match.Index, match.Length);
                            expression = expression.Insert(match.Index, Sum(firstNumber, secondNumber).ToString("0.##"));
                        }
                        break;
                    case "*":
                        {
                            expression = expression.Remove(match.Index, match.Length);
                            if (firstNumber < 0 && secondNumber < 0)
                                expression = expression.Insert(match.Index, "+" + Multiply(firstNumber,secondNumber).ToString("0.##"));
                            else
                                expression = expression.Insert(match.Index, Multiply(firstNumber, secondNumber).ToString("0.##"));
                        }
                        break;
                    case "/":
                        {
                            if (secondNumber == 0d)
                                throw new DivideByZeroException();

                            expression = expression.Remove(match.Index, match.Length);
                            if (firstNumber < 0 && secondNumber < 0)
                                expression = expression.Insert(match.Index, "+" + Divide(firstNumber,secondNumber).ToString("0.##"));
                            else
                                expression = expression.Insert(match.Index, Divide(firstNumber,secondNumber).ToString("0.##"));
                        }
                        break;
                }
            }

            return expression;
        }

        private static string DoubleToFraction(string pattern, double value)
        {
            Regex regex = new Regex(pattern);
            if(regex.IsMatch(value.ToString()))
            {
                var absValue = Math.Abs(value).ToString();
                var decimals = absValue.Length - (absValue.IndexOf('.') - 1);

                var valueToPowerOf = value * Math.Pow(100, decimals);
                var divisor = GreatestCommonDonominator((int)valueToPowerOf, (int)Math.Pow(100, decimals));

                return $"{valueToPowerOf / divisor}/{(Math.Pow(100, decimals) / divisor)}";
            }
            return value.ToString();
        }

        static int GreatestCommonDonominator(int valueToPowerOf, int toPowerOf)
        {
            if (toPowerOf == 0) return valueToPowerOf;
            else return GreatestCommonDonominator(toPowerOf, valueToPowerOf % toPowerOf);
        }

        private static double Divide(double firstNumber, double secondNumber) => firstNumber / secondNumber;

        private static double Multiply(double firstNumber, double secondNumber) => firstNumber * secondNumber;

        private static double Subtract(double firstNumber, double secondNumber) => firstNumber - secondNumber;

        private static double Sum(double firstNumber, double secondNumber) => firstNumber + secondNumber;
    }
}

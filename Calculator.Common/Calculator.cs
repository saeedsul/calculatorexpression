using System;
using System.Text.RegularExpressions;

namespace Calculator.Common
{
    public static class Calculator
    {
        private static readonly string multiplyDividePattern = @"(?<firstNumber>[-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?)))(?<symbol>[*/])(?<secondNumber>[-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?)))";
        //@"(?<firstNumber>(\d+)|(\d+\.\d+))(?<symbol>[*\/])(?<secondNumber>(\d+)|(\d+\.\d+))";
        private static readonly string addSubtractPatten = @"(?<firstNumber>(?<![*/\d-])([-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?))))(?<symbol>[-+])(?<secondNumber>[-]?((∞)|(((\d+,\d+)|(\d+\.\d+)|(\d+))(E[+-]\d+)?))(?![/*\d]))";
        //@"(?<firstNumber>(\d+)|(\d+\.\d+))(?<symbol>[-+])(?<secondNumber>(\d+)|(\d+\.\d+))";

        private static readonly string decimalsPattern = @"^\d+\.?\d+$";
        private static readonly string allowedPattern = @"^\s*(\d+)(?:\s*([-+*\/])\s*(?:\d+)\s*)+$";

        public static string Calculate(string expression)
        {
            var regex = new Regex(allowedPattern);
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
            var regex = new Regex(pattern);

            while (regex.IsMatch(expression))
            {
                var match = regex.Match(expression);
                var firstNumber = double.Parse(match.Groups["firstNumber"].Value);
                var secondNumber = double.Parse(match.Groups["secondNumber"].Value);

                switch (match.Groups["symbol"].Value)
                {
                    case "-":
                        {
                            expression = expression.Remove(match.Index, match.Length);
                            expression = expression.Insert(match.Index, $"{Subtract(firstNumber, secondNumber):0.##}");
                        }
                        break;
                    case "+":
                        {
                            expression = expression.Remove(match.Index, match.Length);
                            expression = expression.Insert(match.Index, $"{Sum(firstNumber, secondNumber):0.##}");
                        }
                        break;
                    case "*":
                        {
                            expression = expression.Remove(match.Index, match.Length);
                            if (firstNumber < 0 && secondNumber < 0)
                                expression = expression.Insert(match.Index, "+" + $"{Multiply(firstNumber, secondNumber):0.##}");
                            else
                                expression = expression.Insert(match.Index, $"{Multiply(firstNumber, secondNumber):0.##}");
                        }
                        break;
                    case "/":
                        {
                            if (secondNumber == 0d)
                                throw new DivideByZeroException();

                            expression = expression.Remove(match.Index, match.Length);
                            if (firstNumber < 0 && secondNumber < 0)
                                expression = expression.Insert(match.Index, "+" + $"{Divide(firstNumber, secondNumber):0.##}");
                            else
                                expression = expression.Insert(match.Index, $"{Divide(firstNumber, secondNumber):0.##}");
                        }
                        break;
                }
            }

            return expression;
        }

        private static string DoubleToFraction(string pattern, double value)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(value.ToString())) return value.ToString();

            var absValue = Math.Abs(value).ToString();
            var decimals = absValue.Length - (absValue.IndexOf('.') - 1);

            var valueToPowerOf = value * Math.Pow(100, decimals);
            var divisor = GreatestCommonDinominator((int)valueToPowerOf, (int)Math.Pow(100, decimals));

            return $"{valueToPowerOf / divisor}/{(Math.Pow(100, decimals) / divisor)}";
        }

        static int GreatestCommonDinominator(int valueToPowerOf, int toPowerOf)
        {
            return toPowerOf == 0 ? valueToPowerOf : GreatestCommonDinominator(toPowerOf, valueToPowerOf % toPowerOf);
        }

        private static double Divide(double firstNumber, double secondNumber) => firstNumber / secondNumber;

        private static double Multiply(double firstNumber, double secondNumber) => firstNumber * secondNumber;

        private static double Subtract(double firstNumber, double secondNumber) => firstNumber - secondNumber;

        private static double Sum(double firstNumber, double secondNumber) => firstNumber + secondNumber;
    }
}

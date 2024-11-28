using System.Buffers;
using System.ComponentModel.Design;
using System.Data;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("To calculate a value enter numbers and operators or \"quit\" if you want to end the program.\n" +
                "Input Should contains only digits and operators. You can write negative numbers too.\n" +
                "For example input can be like this : \"123+1-3*4+12/6+-1\".");

            while (start()) ;
        }




        // This method is responsible for one input. Reads input checks it and prints the result.
        private static bool start()
        {

            Console.Write("Input : ");
            String ln = getValidInput();
            if (ln.Equals("quit")) return false;

            Stack<double> numbers = new Stack<double>();
            Stack<char> operators = new Stack<char>();
            Dictionary<char, int> opVal = new Dictionary<char, int>();
            opVal['-'] = 1;
            opVal['+'] = 1;
            opVal['*'] = 2;
            opVal['/'] = 2;

            evaluate(ln, numbers, operators, opVal);
            Console.WriteLine("The result is : " + numbers.Peek());
            return true;

        }



        //This method evaluate full equation.
        private static void evaluate(String ln, Stack<double> numbers, Stack<char> operators, Dictionary<char, int> opVal)
        {
            int curr = 0;
            bool isNegative = false;
            for (int i = 0; i < ln.Length; i++)
            {
                buildNumbers(ln, i, ref curr, ref isNegative, numbers, operators, opVal);
            }
            while (operators.Count > 0) calc(numbers, operators);

        }
        // This method reads one character and build number or saves it as an operator.
        private static void buildNumbers(string ln, int i, ref int curr, ref bool isNegative, Stack<double> numbers, Stack<char> operators, Dictionary<char, int> opVal)
        {
            if (ln[i] == '-' && (i == 0 || !Char.IsDigit(ln[i - 1]))) isNegative = true;

            else if (Char.IsDigit(ln[i]))
            {
                curr *= 10;
                curr += (ln[i] - '0');
                if (i == ln.Length - 1 || ln[i + 1] < '0' || ln[i + 1] > '9')
                {
                    if (isNegative) curr *= -1;
                    numbers.Push(curr);
                    curr = 0;
                    isNegative = false;
                }
            }
            else
            {
                while (operators.Count > 0 && opVal[ln[i]] <= opVal[operators.Peek()]) calc(numbers, operators);
                operators.Push(ln[i]);
            }
        }



        // This method returns string. After checking it on all the validations.
        private static String getValidInput()
        {
            while (true)
            {
                String str = Console.ReadLine();
                if (str.Equals("quit")) return str;

                String opers = "+-*/";
                bool isValid = checkFAL(str);
                for (int i = 1; i < str.Length - 1; i++)
                {
                    if (opers.Contains(str[i]) && opers.Contains(str[i + 1]))
                    {
                        if (opers.Contains(str[i - 1]) || str[i + 1] != '-') isValid = false;
                    }
                    if (str[i] == '/' && !(i == str.Length - 1) && str[i + 1] == '0') isValid = false;
                    if (!Char.IsDigit(str[i]) && !opers.Contains(str[i]))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid) return str;
                Console.Write("Please Enter valid input : ");

            }

        }


        // This method checks first and last characters of string. (FAL - First And Last) 
        private static bool checkFAL(String str)
        {
            if (str.Length == 0) return false;

            String opers = "+-*/";
            if (opers.Contains(str[str.Length - 1]) || (opers.Contains(str[0]) && str[0] != '-')) return false;
            if (!Char.IsDigit(str[str.Length - 1]) || (!Char.IsDigit(str[0]) && str[0] != '-')) return false;

            return true;
        }


        //This method evaluate one operation
        private static void calc(Stack<double> numbers, Stack<char> operators)
        {
            double second = numbers.Pop();
            double first = numbers.Pop();
            char op = operators.Pop();

            if (op == '+') numbers.Push(first + second);
            else if (op == '-') numbers.Push(first - second);
            else if (op == '*') numbers.Push(first * second);
            else numbers.Push(first / second);
        }

    }
}

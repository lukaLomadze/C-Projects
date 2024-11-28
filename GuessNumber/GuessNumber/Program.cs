using System.Diagnostics;

namespace GuessNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            while (start(rand)) ;
        }
        
        
        // This method is jus one play. And is called until user wants to quit.
        private static bool start(Random rand)
        {
            Console.Write("To start playing please enter min and max value of the number you will have to guess.\nIf you want to quit enter \"quit\" or anything otherwise : ");
            if (Console.ReadLine().Equals("quit")) return true;
            Console.Write("Enter Min value : ");
            int minVal = getValidNumber();
            Console.Write("Enter Max value : ");
            int maxVal = getValidNumber();
            if (minVal > maxVal)
            {
                Console.WriteLine("Max value should be greater than Min value, try again!");
                return true;
            }
            play(rand, minVal,maxVal);
            return false;
        }
        //This method is one real game. Randomize target and interacts with player.
        private static void play(Random rand, int minVal, int maxVal)
        { 
            int target = rand.Next(minVal, maxVal);
            int count = 0;
            while (true)
            {
                Console.Write("Guess the number : ");
                int tried = getValidNumber();
                count++;
                if (tried == target)
                {
                    Console.WriteLine($"Congratulations, you guessed the number {target} , it took you {count} step.");
                    break;
                }
                else if (tried < target)
                {
                    Console.WriteLine("No, this number is less then the target.");
                }
                else
                {
                    Console.WriteLine("No, this number is greater then the target.");
                }

            }


        }
        // This method reads valid number from the user.
        private static int getValidNumber()
        {
            int n ;
            while(true) {
                if (int.TryParse(Console.ReadLine(), out n)) return n;
                Console.Write("Please enter Valid number : ");
            }

        }
    }
}

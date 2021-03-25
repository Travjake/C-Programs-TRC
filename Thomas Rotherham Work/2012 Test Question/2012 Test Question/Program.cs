using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2012_Test_Question
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1) 2012 Question\n   2) 2013 Question\n      3) 2014 Question");
            ConsoleKeyInfo currentKey = Console.ReadKey(true);
            switch (currentKey.Key)
            {
                case ConsoleKey.D1:
            int ans = 0;
            int col = 8;
            while (col >= 1)
            {
                Console.WriteLine("Enter Bit Value (From left): ");
                int bv = Convert.ToInt16(Console.ReadLine());
                ans = ans + (col * bv);
                col = col / 2;
            }
            Console.WriteLine("The decimal value is: " + ans);
            Console.ReadLine();
                    break;
            
                case ConsoleKey.D2:
                    Console.WriteLine("Player One Enter the Chosen Number(Between 1-10): ");
                    int numtoguess = Convert.ToInt16(Console.ReadLine());
                    while(numtoguess < 1 && numtoguess > 10)
                    {
                        Console.WriteLine("Not a valid number enter another: ");
                        numtoguess = Convert.ToInt16(Console.ReadLine());
                    }
                    int guess = 0, numofguesses = 0;
                    while(guess != numtoguess && numofguesses < 5)
                    {
                        Console.WriteLine("Player Two Have a Guess");
                        guess = Convert.ToInt16(Console.ReadLine());
                        numofguesses++;
                    }
                    if (guess == numtoguess) { Console.WriteLine("Player Two Wins"); }
                    else { Console.WriteLine("Player One Wins"); }
                    break;
            }
        }
    }
}

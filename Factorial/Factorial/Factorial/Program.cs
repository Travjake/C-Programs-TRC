using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a number: ");
            long number = Convert.ToInt64(Console.ReadLine());
            long fact = GetFactorial(number);
            Console.WriteLine("{0} is {1}", number, fact);
            Console.ReadKey();
        }

        private static long GetFactorial(long number)
        {
            if (number == 0)
            {
            return 1;
            }
            return number * GetFactorial(number - 1);
        }
    }
}

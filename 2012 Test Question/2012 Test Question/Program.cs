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
        }
    }
}

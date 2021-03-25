using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopStockControl
{
    class Program
    {
        int itemSize = 0;
        string[] ItemArray = new string[6];
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("StockControl v1.0: By Travis Chapple\n1) Add New Item \n2) Remove Item \n3) Show Current Stock Levels \n4) Re-Order Infomation  \n5) Simulation Infomation");
                ConsoleKeyInfo input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine(ItemArray);
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();

                        break;
                    case ConsoleKey.D3:
                        Console.Clear();

                        break;
                    case ConsoleKey.D4:
                        Console.Clear();

                        break;
                    case ConsoleKey.D5:
                        Console.Clear();

                        break;
                }

            }
        }


    }
}

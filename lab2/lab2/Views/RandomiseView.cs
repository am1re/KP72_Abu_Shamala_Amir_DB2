using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Views
{
    static class RandomiseView
    {
        public static int ShowRandomise()
        {
            Console.Clear();
            Console.WriteLine("The randomising will destroy all data in database.");
            var a = GetYesNo();
            if (!a)
                return -1;
            return GetNum();
        }

        private static bool GetYesNo()
        {
            Console.WriteLine("Are you ready to proceed?(Y/N)");
            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                if (key == 'Y' || key == 'y' || key == 'N' || key == 'n')
                    return key == 'Y' || key == 'y';
                Console.WriteLine("Wrong input!");
            }
        }

        private static int GetNum()
        {
            Console.WriteLine("\n\rInput number from 10 to 10000");
            int number;
            while (!int.TryParse(Console.ReadLine(), out number)
                   || number < 10 || number > 10000)
                Console.WriteLine("Wrong input!");
            Console.WriteLine("Randomising...");
            return number;
        }
    }
}

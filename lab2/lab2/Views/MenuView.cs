using System;

namespace lab2.Views
{
    internal enum MenuCommands
    {
        Exit = 0,
        Crud,
        Random,
        FullTextSearch
    }

    internal static class MenuView
    {
        public static MenuCommands ShowMain()
        {
            Console.Clear();
            Console.WriteLine("1 - CRUD for entities");
            Console.WriteLine("2 - random generation");
            Console.WriteLine("3 - full text search");
            Console.WriteLine("0 - exit");
            return (MenuCommands) GetNum(0, 3);
        }

        public static Entities ShowEntities()
        {
            Console.Clear();
            Console.WriteLine("1 - Accident");
            Console.WriteLine("2 - Car");
            Console.WriteLine("3 - Person");
            Console.WriteLine("0 - back");
            return (Entities) GetNum(0, 3);
        }

        private static int GetNum(int downLimit, int upLimit)
        {
            int number;
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out number) 
                   || number < downLimit || number > upLimit)
                Console.WriteLine("Wrong input!");
            return number;
        }
    }
}

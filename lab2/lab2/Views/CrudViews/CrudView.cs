using System;
using System.Collections.Generic;
using System.Data;
using ConsoleTableExt;
using lab2.Database.DAO;

namespace lab2.Views.CrudViews
{
    public abstract class CrudView<T>
    {
        private readonly string _entityName;

        protected CrudView(string entityName) => _entityName = entityName;
        public CrudOperations Begin(List<T> data, int page)
        {
            Console.Clear();
            Console.WriteLine($"{_entityName}:");
            ConsoleTableBuilder.From(DataToDataTable(data))
                                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                                .ExportAndWriteLine();
            Console.WriteLine($"Page {page + 1}");
            Console.WriteLine("<,> - left, right");
            Console.WriteLine("Operations:");
            Console.WriteLine("1 - Create");
            Console.WriteLine("2 - Get");
            Console.WriteLine("3 - Update");
            Console.WriteLine("4 - Delete");
            Console.WriteLine("5 - Search");
            Console.WriteLine("0 - back");
            return GetOperation();
        }

        public abstract T Create();

        public long Read()
        {
            Console.WriteLine("\r\nInput id");
            long num;
            while (!long.TryParse(Console.ReadLine(), out num) && num <= 0)
                Console.WriteLine("Wrong input!");
            return num;
        }

        public abstract void ShowReadResult(T data);

        public abstract T Update(T entity);

        public void OperationStatusOutput(bool status)
        {
            Console.WriteLine(status ? "Operation successful!" : "Operation failed!");
            Console.ReadKey();
        }

        protected abstract DataTable DataToDataTable(List<T> data);

        private CrudOperations GetOperation()
        {
            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                if (key == '<')
                    return CrudOperations.PageLeft;
                if (key == '>')
                    return CrudOperations.PageRight;
                if (int.TryParse(key.ToString(), out var res) && res >= 0 && res <= 5)
                    return (CrudOperations) res;
                Console.WriteLine("Wrong input!");
            }
        }

        
    }
}
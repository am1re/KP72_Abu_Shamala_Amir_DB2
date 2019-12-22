using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ConsoleTableExt;
using lab2.Database.DAO;
using lab2.Models;

namespace lab2.Views.CrudViews
{
    public class AccidentView : CrudView<Accident>
    {
        private readonly DAO<Person> _personDAO;

        public AccidentView(DAO<Person> personDAO) : base("Accidents")
        {
            _personDAO = personDAO;
        }

        public override Accident Create()
        {
            Console.WriteLine("\n\rInput location");
            var location = Console.ReadLine();

            Console.WriteLine("Input date");
            DateTime date;
            var date_input = Console.ReadLine();
            while (!DateTime.TryParse(date_input, out date))
            {
                Console.WriteLine("Not a valid date, try again.");
                date_input = Console.ReadLine();
            }

            Console.WriteLine("Input damage amount");
            var dmg = Console.ReadLine();
            Console.WriteLine("Input damage person id");
            var person = GetPerson();
            return new Accident(-1, location, date, dmg, person);
        }

        public override void ShowReadResult(Accident data)
        {
            Console.WriteLine("Result:");
            ConsoleTableBuilder.From(DataToDataTable(new List<Accident>{data}))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.ReadKey();
        }

        public override Accident Update(Accident entity)
        {
            Console.WriteLine($"Input location. Old one: {entity.Location}");
            entity.Location = Console.ReadLine();

            Console.WriteLine($"Input date. Old one: {entity.Date.ToString()}");
            DateTime date;
            var date_input = Console.ReadLine();
            while (!DateTime.TryParse(date_input, out date))
            {
                Console.WriteLine("Not a valid date, try again.");
                date_input = Console.ReadLine();
            }
            entity.Date = date;

            Console.WriteLine($"Input damage amount. Old one: {entity.Damage_Amount}");
            entity.Damage_Amount = Console.ReadLine();

            return entity;
        }

        public (string str, int from, int to) Search()
        {
            Console.WriteLine("\r\nInput location");
            var loc = Console.ReadLine();
            Console.WriteLine("Car VIN range. From:");
            var from = (int)GetNum();
            Console.WriteLine("Car VIN range. To:");
            var to = (int)GetNum();
            return (loc, from, to);
        }

        protected override DataTable DataToDataTable(List<Accident> data)
        {
            var dataTable = new DataTable("Accidents");
            dataTable.Columns.Add(new DataColumn("Record-Number", typeof(long)));
            dataTable.Columns.Add(new DataColumn("Location", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("Damage-Amount", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Person-Id", typeof(long)));

            if (data.Count == 0)
            {
                var row = dataTable.NewRow();
                row.ItemArray = new object[]{-1, "Empty", DateTime.MinValue, "Empty", -1};
                dataTable.Rows.Add(row);
            }
            else
                foreach (var el in data)
                {
                    var row = dataTable.NewRow();
                    row.ItemArray = new object[]
                    {
                        el.Record_Number,
                        el.Location,
                        el.Date,
                        el.Damage_Amount,
                        el.Person.Id
                    };
                    dataTable.Rows.Add(row);
                }

            return dataTable;
        }

        private Person GetPerson()
        {
            while (true)
            {
                var person = _personDAO.Get(GetNum());
                if (person is null)
                    Console.WriteLine("No such person!");
                else
                    return person;
            }
        }

        private static long GetNum()
        {
            long number;
            while (!long.TryParse(Console.ReadLine(), out number)
                   || number <= 0)
                Console.WriteLine("Wrong input!");
            return number;
        }
    }
}

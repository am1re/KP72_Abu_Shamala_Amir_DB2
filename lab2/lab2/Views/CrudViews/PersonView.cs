using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ConsoleTableExt;
using lab2.Database.DAO;
using lab2.Models;

namespace lab2.Views.CrudViews
{
    class PersonView : CrudView<Person>
    {
        private readonly DAO<Car> _carDAO;

        public PersonView(DAO<Car> carDAO) : base("Persons")
        {
            _carDAO = carDAO;
        }

        public override Person Create()
        {
            Console.WriteLine("\n\rInput Name:");
            var name = Console.ReadLine();
            Console.WriteLine("Input address:");
            var address = Console.ReadLine();
            Console.WriteLine("Input phone:");
            var phone = Console.ReadLine();
            Console.WriteLine("Input car VIN:");
            var car = GetCar();
            return new Person(-1, address, name, phone, car);
        }

        public override void ShowReadResult(Person data)
        {
            Console.WriteLine("Result:");
            ConsoleTableBuilder.From(DataToDataTable(new List<Person> { data }))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.ReadKey();
        }

        public override Person Update(Person entity)
        {
            Console.WriteLine($"\n\rInput address. Old value: {entity.Address}");
            entity.Address = Console.ReadLine();
            Console.WriteLine($"Input phone. Old value: {entity.Phone}");
            entity.Phone = Console.ReadLine();
            Console.WriteLine($"Input name. Old value: {entity.Name}");
            entity.Name = Console.ReadLine();

            return entity;
        }

        public string Search()
        {
            Console.WriteLine("\r\nInput name");
            return Console.ReadLine();
        }

        protected override DataTable DataToDataTable(List<Person> data)
        {
            var dataTable = new DataTable("Persons");
            dataTable.Columns.Add(new DataColumn("Id", typeof(long)));
            dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Phone", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Address", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Car VIN", typeof(long)));
            //dataTable.Columns.Add(new DataColumn("Car Year", typeof(int)));
            //dataTable.Columns.Add(new DataColumn("Car Model", typeof(string)));

            if (data.Count == 0)
            {
                var row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", "Empty", "Empty", -1/*, -1, "Empty"*/ };
                dataTable.Rows.Add(row);
            }
            else
                foreach (var el in data)
                {
                    var row = dataTable.NewRow();
                    row.ItemArray = new object[]
                    {
                        el.Id,
                        el.Name,
                        el.Phone,
                        el.Address,
                        el.Car.VIN,
                        //el.Car.Year,
                        //el.Car.Model
                    };
                    dataTable.Rows.Add(row);
                }

            return dataTable;
        }
        private Car GetCar()
        {
            while (true)
            {
                var car = _carDAO.Get(GetNum());
                if (car is null)
                    Console.WriteLine("No such car!");
                else
                    return car;
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

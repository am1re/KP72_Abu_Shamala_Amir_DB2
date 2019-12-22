using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ConsoleTableExt;
using lab2.Models;

namespace lab2.Views.CrudViews
{
    class CarView : CrudView<Car>
    {
        public CarView() : base("Cars")
        {

        }

        public override Car Create()
        {
            Console.WriteLine("\n\rInput VIN (digits only)");
            
            int vin;
            var vin_input = Console.ReadLine();
            while (!Int32.TryParse(vin_input, out vin))
            {
                Console.WriteLine("Not a valid number, try again.");
                vin_input = Console.ReadLine();
            }

            Console.WriteLine("Input model");
            var model = Console.ReadLine();

            Console.WriteLine("Input year (digits only)");

            var year_input = Console.ReadLine();
            int year;
            while (!Int32.TryParse(year_input, out year))
            {
                Console.WriteLine("Not a valid number, try again.");
                year_input = Console.ReadLine();
            }

            return new Car(vin, model, year);
        }

        public override void ShowReadResult(Car data)
        {
            Console.WriteLine("Result:");
            ConsoleTableBuilder.From(DataToDataTable(new List<Car> { data }))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.ReadKey();
        }

        public override Car Update(Car entity)
        {
            Console.WriteLine($"Input model. Old one: {entity.Model}");
            entity.Model = Console.ReadLine();
            
            Console.WriteLine($"Input year. Old one: {entity.Year}");
            var year_input = Console.ReadLine();
            int year;
            while (!Int32.TryParse(year_input, out year))
            {
                Console.WriteLine("Not a valid number, try again.");
                year_input = Console.ReadLine();
            }
            entity.Year = year;

            return entity;
        }

        public string Search()
        {
            Console.WriteLine("\r\nInput model");
            return Console.ReadLine();
        }
        protected override DataTable DataToDataTable(List<Car> data)
        {
            var dataTable = new DataTable("Cars");
            dataTable.Columns.Add(new DataColumn("VIN", typeof(long)));
            dataTable.Columns.Add(new DataColumn("Model", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Year", typeof(int)));
            if (data.Count == 0)
            {
                var row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", -1 };
                dataTable.Rows.Add(row);
            }
            else
                foreach (var el in data)
                {
                    var row = dataTable.NewRow();
                    row.ItemArray = new object[]
                    {
                        el.VIN,
                        el.Model,
                        el.Year
                    };
                    dataTable.Rows.Add(row);
                }

            return dataTable;
        }
    }
}

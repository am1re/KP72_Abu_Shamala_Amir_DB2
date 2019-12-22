using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ConsoleTableExt;
using lab2.Database;

namespace lab2.Views
{
    enum FullTextSearchCommands
    {
        FullPhrase = 1,
        IncludedWord
    }
    static class FullTextSearchView
    {
        public static (FullTextSearchCommands, string) Begin()
        {
            Console.WriteLine("\n\rThe full text search for messages");
            Console.WriteLine("Choose search type:");
            Console.WriteLine("1 - full phrase search");
            Console.WriteLine("2 - included word search");
            var com = GetCommand();
            Console.WriteLine("\n\rInput query");
            var query = Console.ReadLine();
            return (com, query);
        }

        public static void ShowResults(List<SearchResult> results)
        {
            Console.WriteLine("Search results:");
            ConsoleTableBuilder.From(GenerateDataTable(results))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.ReadKey();
        }

        private static FullTextSearchCommands GetCommand()
        {
            int number;
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out number)
                   || number < 1 || number > 2)
                Console.WriteLine("Wrong input!");
            return (FullTextSearchCommands)number;
        }

        private static DataTable GenerateDataTable(List<SearchResult> data)
        {
            var dataTable = new DataTable("Search results");
            dataTable.Columns.Add(new DataColumn("id", typeof(long)));
            dataTable.Columns.Add(new DataColumn("attr", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ts_headline", typeof(string)));
            if (data.Count == 0)
            {
                var row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", "Empty"};
                dataTable.Rows.Add(row);
            }
            else
                foreach (var el in data)
                {
                    var row = dataTable.NewRow();
                    row.ItemArray = new object[]
                    {
                        el.Id,
                        el.Attr,
                        el.TsHeadline
                    };
                    dataTable.Rows.Add(row);
                }

            return dataTable;
        }
    }
}

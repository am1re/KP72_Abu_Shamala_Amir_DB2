using System;
using System.IO;
using lab2.Controllers;
using lab2.Database;
using lab2.Database.DAO;
using lab2.Models;

namespace lab2
{
    internal class Program
    {
        static void Main()
        {
            var connection = new DbConnection("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=testdb1;");
            var controller = new Controller(connection);
            controller.Begin();
        }
    }
}

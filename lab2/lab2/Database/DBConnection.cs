using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace lab2.Database
{
    public class DbConnection
    {
        private readonly NpgsqlConnection _connection;

        public DbConnection(string connectString)
        {
            _connection = new NpgsqlConnection(connectString);
        }

        public NpgsqlConnection Open()
        {
            _connection.Open();
            return _connection;
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}

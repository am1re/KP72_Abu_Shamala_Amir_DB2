using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;

namespace lab2.Database
{
    class FullTextSearch
    {
        private readonly DbConnection _dbConnection;

        public FullTextSearch(DbConnection dbConnection) =>
            _dbConnection = dbConnection;

        public List<SearchResult> GetFullPhrase(string id_col, string atr, string table, string phrase)
        {
            var list = new List<SearchResult>();
            var connection = _dbConnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT {id_col}, {atr}, ts_headline(\"{atr}\", q)" +
                                  $" FROM {table}, phraseto_tsquery('{phrase}') AS q " +
                                  $"WHERE to_tsvector({table}.{atr}) @@ q";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var s = new SearchResult(Convert.ToInt64(reader.GetValue(0).ToString()),
                    reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                list.Add(s);
            }
            _dbConnection.Close();
            return list;
        }


        public List<SearchResult> GetAllWithIncludedWord(string id_col, string atr, string table, string phrase)
        {
            var list = new List<SearchResult>();
            var connection = _dbConnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT {id_col}, {atr} FROM {table} " +
                                  $"WHERE (to_tsvector({table}.{atr}) @@ to_tsquery('{phrase}'))";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var s = new SearchResult(Convert.ToInt64(reader.GetValue(0).ToString()), reader.GetValue(1).ToString(), null);
                list.Add(s);
            }
            _dbConnection.Close();
            return list;
        }
    }
}

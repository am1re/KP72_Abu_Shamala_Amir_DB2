using lab2.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Database.DAO
{
    public class AccidentDAO : DAO<Accident>
    {
        public AccidentDAO(DbConnection db) : base(db) { }

        public override void Clear()
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "TRUNCATE TABLE public.accident RESTART IDENTITY CASCADE";
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public override long Create(Accident entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO public.accident (location, date, \"damage-amount\", person) VALUES (:location, :date, :dmg, :person)";
            command.Parameters.Add(new NpgsqlParameter("location", entity.Location));
            command.Parameters.Add(new NpgsqlParameter("date", NpgsqlDateTime.ToNpgsqlDateTime(entity.Date)));
            command.Parameters.Add(new NpgsqlParameter("dmg", entity.Damage_Amount));
            command.Parameters.Add(new NpgsqlParameter("person", entity.Person.Id));
            command.ExecuteNonQuery();

            command.CommandText = "SELECT \"record-number\" FROM public.accident WHERE location = :loc AND person = :pers";
            command.Parameters.Add(new NpgsqlParameter("loc", entity.Location));
            command.Parameters.Add(new NpgsqlParameter("pers", entity.Person.Id));
            var reader = command.ExecuteReader();

            long id = -1;
            if (reader.Read())
                id = reader.GetInt64(0);

            Dbconnection.Close();

            return id;
        }

        public override void Delete(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM public.accident WHERE \"record-number\" = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public override Accident Get(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT \"record-number\", location, date, \"damage-amount\", \"person-id\", address, name, phone, \"VIN\", \"Model\", \"Year\" " +
                "FROM public.accident " +
                "INNER JOIN public.person ON person.\"person-id\" = accident.person " +
                "INNER JOIN public.car ON car.\"VIN\" = person.car " +
                "WHERE \"record-number\" = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));

            var reader = command.ExecuteReader();
            Accident accident = null;

            if (reader.Read())
            {
                accident = new Accident(reader.GetInt64(0), reader.GetString(1), reader.GetTimeStamp(2).ToDateTime(), reader.GetString(3),
                            new Person(reader.GetInt64(4), reader.GetString(5), reader.GetString(6), reader.GetString(7),
                                new Car(reader.GetInt64(8), reader.GetString(9), reader.GetInt32(10)))
                           );
            }

            Dbconnection.Close();
            return accident;
        }

        public override List<Accident> Get(int page)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT \"record-number\", location, date, \"damage-amount\", \"person-id\", address, name, phone, \"VIN\", \"Model\", \"Year\" " +
                "FROM public.accident " +
                "INNER JOIN public.person ON person.\"person-id\" = accident.person " +
                "INNER JOIN public.car ON car.\"VIN\" = person.car " +
                "ORDER BY \"record-number\" " +
                "LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));

            var reader = command.ExecuteReader();
            var accidents = new List<Accident>();

            while (reader.Read())
            {
                accidents.Add(new Accident(reader.GetInt64(0), reader.GetString(1),
                                reader.GetTimeStamp(2).ToDateTime(), reader.GetString(3),
                                new Person(reader.GetInt64(4), reader.GetString(5), reader.GetString(6), reader.GetString(7),
                                    new Car(reader.GetInt64(8), reader.GetString(9), reader.GetInt32(10)))
                              ));
            }

            Dbconnection.Close();
            return accidents;
        }

        public override void Update(Accident entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "UPDATE public.accident SET location = :location, date = :date, " +
                "\"damage-amount\" = :dmg, person = :personid " +
                "WHERE \"record-number\" = :rec_id";
            command.Parameters.Add(new NpgsqlParameter("location", entity.Location));
            command.Parameters.Add(new NpgsqlParameter("date", NpgsqlDateTime.ToNpgsqlDateTime(entity.Date)));
            command.Parameters.Add(new NpgsqlParameter("dmg", entity.Damage_Amount));
            command.Parameters.Add(new NpgsqlParameter("personid", entity.Person.Id));
            command.Parameters.Add(new NpgsqlParameter("rec_id", entity.Record_Number));
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public Accident Search(string str, int from, int to)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT \"record-number\", location, date, \"damage-amount\", \"person-id\", address, name, phone, \"VIN\", \"Model\", \"Year\" " +
                "FROM public.accident " +
                "INNER JOIN public.person ON person.\"person-id\" = accident.person " +
                "INNER JOIN public.car ON car.\"VIN\" = person.car " +
                "WHERE location = :location AND person.car > :from AND person.car < :to";

            command.Parameters.Add(new NpgsqlParameter("location", str));
            command.Parameters.Add(new NpgsqlParameter("from", from));
            command.Parameters.Add(new NpgsqlParameter("to", to));

            var reader = command.ExecuteReader();
            Accident accident = null;

            if (reader.Read())
            {
                accident = new Accident(reader.GetInt64(0), reader.GetString(1),
                            reader.GetTimeStamp(2).ToDateTime(), reader.GetString(3),
                            new Person(reader.GetInt64(4), reader.GetString(5), reader.GetString(6), reader.GetString(7),
                                new Car(reader.GetInt64(8), reader.GetString(9), reader.GetInt32(10)))
                            );
            }

            Dbconnection.Close();
            return accident;
        }
    }
}

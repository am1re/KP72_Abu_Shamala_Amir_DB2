using lab2.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Database.DAO
{
    public class PersonDAO : DAO<Person>
    {
        public PersonDAO(DbConnection db) : base(db) { }

        public override void Clear()
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "TRUNCATE TABLE public.person RESTART IDENTITY CASCADE";
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public override long Create(Person entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO public.person (address, name, phone, car) VALUES (:address, :name, :phone, :car)";
            command.Parameters.Add(new NpgsqlParameter("address", entity.Address));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
            command.Parameters.Add(new NpgsqlParameter("phone", entity.Phone));
            command.Parameters.Add(new NpgsqlParameter("car", entity.Car.VIN));
            command.ExecuteNonQuery();

            Dbconnection.Close();

            connection = Dbconnection.Open();
            command = connection.CreateCommand();

            command.CommandText = "SELECT \"person-id\" FROM public.person WHERE car = :car_vin AND name = :name";
            command.Parameters.Add(new NpgsqlParameter("car_vin", entity.Car.VIN));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
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

            command.CommandText = "DELETE FROM public.person WHERE \"person-id\" = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public override Person Get(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = 
                "SELECT \"person-id\", address, name, phone, \"VIN\", \"Model\", \"Year\" " +
                "FROM public.person " +
                "INNER JOIN public.car ON car.\"VIN\" = person.car " +
                "WHERE \"person-id\" = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));

            var reader = command.ExecuteReader();
            Person person = null;

            if (reader.Read())
            {
                person = new Person(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                            new Car(reader.GetInt64(4), reader.GetString(5), reader.GetInt32(6)));
            }

            Dbconnection.Close();
            return person;
        }

        public override List<Person> Get(int page)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT \"person-id\", address, name, phone, \"VIN\", \"Model\", \"Year\" " +
                "FROM public.person " +
                "INNER JOIN public.car ON car.\"VIN\" = person.car " +
                "ORDER BY \"person-id\" " +
                "LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));

            var reader = command.ExecuteReader();
            var persons = new List<Person>();

            while (reader.Read())
            {
                persons.Add(new Person(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                new Car(reader.GetInt64(4), reader.GetString(5), reader.GetInt32(6))
                            ));
            }

            Dbconnection.Close();
            return persons;
        }

        public override void Update(Person entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "UPDATE public.person SET address = :address, name = :name, " +
                "phone = :phone, car = :carid " +
                "WHERE \"person-id\" = :pers_id";
            command.Parameters.Add(new NpgsqlParameter("address", entity.Address));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
            command.Parameters.Add(new NpgsqlParameter("phone", entity.Phone));
            command.Parameters.Add(new NpgsqlParameter("carid", entity.Car.VIN));
            command.Parameters.Add(new NpgsqlParameter("pers_id", entity.Id));
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public Person Search(string str)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText =
                "SELECT \"person-id\", address, name, phone, \"VIN\", \"Model\", \"Year\" " +
                "FROM public.person " +
                "INNER JOIN public.car ON car.\"VIN\" = person.car " +
                "WHERE Name = :name";

            command.Parameters.Add(new NpgsqlParameter("name", str));

            var reader = command.ExecuteReader();
            Person person = null;

            if (reader.Read())
            {
                person = new Person(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), 
                            reader.GetString(3),
                            new Car(reader.GetInt64(4), reader.GetString(5), reader.GetInt32(6))
                         );
            }

            Dbconnection.Close();
            return person;
        }
    }
}

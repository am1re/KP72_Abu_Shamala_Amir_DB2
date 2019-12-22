using lab2.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Database.DAO
{
    public class CarDAO : DAO<Car>
    {
        public CarDAO(DbConnection db) : base(db) { }

        public override void Clear()
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            
            command.CommandText = "TRUNCATE TABLE public.car RESTART IDENTITY CASCADE";
            command.ExecuteNonQuery();
            
            Dbconnection.Close();
        }

        public override long Create(Car entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO public.car (\"VIN\", \"Model\", \"Year\") VALUES (:vin, :model, :year)";
            command.Parameters.Add(new NpgsqlParameter("vin", entity.VIN));
            command.Parameters.Add(new NpgsqlParameter("model", entity.Model));
            command.Parameters.Add(new NpgsqlParameter("year", entity.Year));
            command.ExecuteNonQuery();
            
            Dbconnection.Close();

            return entity.VIN;
        }

        public override void Delete(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM public.car WHERE \"VIN\" = :vin";
            command.Parameters.Add(new NpgsqlParameter("vin", id));
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public override Car Get(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM public.car WHERE \"VIN\" = :vin";
            command.Parameters.Add(new NpgsqlParameter("vin", id));

            var reader = command.ExecuteReader();
            Car car = null;
            
            if (reader.Read())
            {
                car = new Car(reader.GetInt64(0), reader.GetString(1), reader.GetInt32(2));
            }
            
            Dbconnection.Close();
            return car;
        }

        public override List<Car> Get(int page)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM public.car LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));
            
            var reader = command.ExecuteReader();
            var cars = new List<Car>();
            
            while (reader.Read())
            {
                cars.Add(new Car(reader.GetInt64(0), reader.GetString(1), reader.GetInt32(2)));
            }

            Dbconnection.Close();
            return cars;
        }

        public override void Update(Car entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();

            command.CommandText = "UPDATE public.car SET \"Model\" = :model, \"Year\" = :Year WHERE \"VIN\" = :vin";
            command.Parameters.Add(new NpgsqlParameter("vin", entity.VIN));
            command.Parameters.Add(new NpgsqlParameter("model", entity.Model));
            command.Parameters.Add(new NpgsqlParameter("year", entity.Year));
            command.ExecuteNonQuery();

            Dbconnection.Close();
        }

        public Car Search(string str)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            
            command.CommandText = "SELECT * FROM public.car WHERE \"Model\" = :model";
            command.Parameters.Add(new NpgsqlParameter("model", str));
            
            var reader = command.ExecuteReader();
            Car car = null;

            if (reader.Read())
            {
                car = new Car(reader.GetInt64(0), reader.GetString(1), reader.GetInt32(2));
            }

            Dbconnection.Close();
            return car;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lab2.Database.DAO;
using lab2.Models;

namespace lab2.Database
{
    class Randomiser
    {
        private readonly DAO<Accident> _accidentDAO;
        private readonly DAO<Car> _carDAO;
        private readonly DAO<Person> _personDAO;
        private readonly Random _random = new Random();

        public Randomiser(DAO<Accident> accidentDAO, DAO<Car> carDAO, DAO<Person> personDAO)
        {
            _accidentDAO = accidentDAO;
            _carDAO = carDAO;
            _personDAO = personDAO;
        }

        public void Randomise(int number)
        {
            _personDAO.Clear();
            _accidentDAO.Clear();
            _carDAO.Clear();
            GenerateDB(number);
        }

        private void GenerateDB(int number)
        {
            for (int i = 0; i < number; i++)
            {
                var car = new Car(_random.Next(1000,10000), RandomAlphaNumericString(16), _random.Next(1940, 2019));
                var carId = _carDAO.Create(car);

                var person = new Person(-1, RandomAlphaNumericString(16), RandomAlphaNumericString(16), 
                    RandomAlphaNumericString(16), car);
                var personId = _personDAO.Create(person);
                person.Id = personId;

                var accident = new Accident(-1, RandomAlphaNumericString(16), 
                    DateTime.Today.AddDays(_random.Next(-365, 365)), RandomAlphaNumericString(16), person);
                var accidentId = _accidentDAO.Create(accident);
                accident.Record_Number = accidentId;
            }
        }

        private string RandomAlphaNumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}

using System;

namespace lab2.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Car Car { get; set; }

        public Person(long id, string address, string name, string phone, Car car)
        {
            Id = id;
            Address = address;
            Name = name;
            Phone = phone;
            Car = car;
        }
    }
}

using System;

namespace lab2.Models
{
    public class Accident
    {
        public long Record_Number { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Damage_Amount { get; set; }
        public Person Person { get; set; }

        public Accident(long record_Number, string location, DateTime date, string damage_Amount, Person person)
        {
            Record_Number = record_Number;
            Location = location;
            Date = date;
            Damage_Amount = damage_Amount;
            Person = person;
        }
    }
}

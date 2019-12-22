namespace lab2.Models
{
    public class Car
    {
        public long VIN { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public Car(long vin, string model, int year)
        {
            VIN = vin;
            Model = model;
            Year = year;
        }
    }
}

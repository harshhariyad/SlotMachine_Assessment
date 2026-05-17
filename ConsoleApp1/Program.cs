using System;
using Microsoft.VisualBasic;

namespace ConsoleApp1
{
    class Car
    {
        public string model;
        public int year;
        public string color;

        public Car(string model, int year, string color)
        {
            this.model = model;
            this.year = year;
            this .color = color;
        }
        static void Main(string[] args)
        {
            Car myCar = new Car("Toyota Camry", 2020, "Red");
            Console.WriteLine("Model: " + myCar.model);
            Console.WriteLine("Year: " + myCar.year);
            Console.WriteLine("Color: " + myCar.color);
        }
    }
}
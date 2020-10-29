using System;

using System.Globalization;

namespace Course231.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }

        public override string ToString()
        {

            return Id
                + ", "
                + Name
                + ", "
                + Price.ToString("C", new CultureInfo("pt-BR"))
                + ", "
                + Category.Name
                + ", "
                + Category.Tier;
        }
    }
}

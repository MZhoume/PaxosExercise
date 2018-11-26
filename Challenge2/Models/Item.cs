using System;

namespace Challenge2.Models
{
    /// <summary>
    /// <see cref="Item"/> class contains the information for each item. Including the name and price.
    /// </summary>
    public class Item
    {
        public string Name { get; set; }

        // Let's assume the user is not a billionaire and the price would not exceed 2^32 - 1
        public int Price { get; set; }

        public static Item Parse(string entry)
        {
            if (string.IsNullOrEmpty(entry))
            {
                throw new ArgumentException(nameof(entry));
            }

            var values = entry.Split(", ");
            if (values.Length != 2)
            {
                throw new ArgumentException("Malformed input entry.");
            }

            if (!int.TryParse(values[1], out var price))
            {
                throw new ArgumentException("Price is not valid.");
            }

            var item = new Item
            {
                Name = values[0],
                Price = price
            };

            return item;
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Price}";
        }
    }
}

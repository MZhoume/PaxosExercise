using System;
using System.Collections.Generic;
using System.IO;
using Challenge2.Models;

namespace Challenge2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Please enter correct arguments.");
                Environment.Exit(1);
            }

            var fileName = Path.Combine("data", args[0]);
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Input file cannot be found under 'data' folder.");
                Environment.Exit(1);
            }

            if (!int.TryParse(args[1], out var target))
            {
                Console.WriteLine("Incorrect target price value.");
                Environment.Exit(1);
            }

            switch (args[2])
            {
                case "2":
                    var items2 = GetItems(fileName, target);

                    var (foundPair, first, second) = FindPair(items2, target);
                    if (foundPair)
                    {
                        Console.WriteLine($"{first}, {second}");
                    }
                    else
                    {
                        Console.WriteLine("Not possible");
                    }

                    break;
                case "3":
                    var items3 = GetItems(fileName, target);

                    var (foundTrio, one, two, three) = FindTrio(items3, target);
                    if (foundTrio)
                    {
                        Console.WriteLine($"{one}, {two}, {three}");
                    }
                    else
                    {
                        Console.WriteLine("Not possible");
                    }

                    break;
                default:
                    Console.WriteLine("Incorrect mode.");
                    Environment.Exit(1);

                    break;
            }
        }

        public static (bool foundPair, Item first, Item second) FindPair(List<Item> items, int target)
        {
            int lo = 0, hi = items.Count - 1;
            int f = -1, s = -1, diff = int.MaxValue;
            while (lo < hi)
            {
                var sum = items[lo].Price + items[hi].Price;
                if (sum == target)
                {
                    return (true, items[lo], items[hi]);
                }
                else if (sum > target)
                {
                    hi -= 1;
                }
                else
                {
                    var d = target - sum;
                    if (diff > d)
                    {
                        f = lo;
                        s = hi;
                        diff = d;
                    }

                    lo += 1;
                }
            }

            return diff == int.MaxValue ? (false, null, null) : (true, items[f], items[s]);
        }

        public static (bool foundTrio, Item one, Item two, Item three) FindTrio(List<Item> items, int target)
        {
            int f = -1, s = -1, t = -1, diff = int.MaxValue;
            for (var i = 0; i < items.Count - 2; i++)
            {
                int lo = i + 1, hi = items.Count - 1;
                while (lo < hi)
                {
                    var sum = items[i].Price + items[lo].Price + items[hi].Price;
                    if (sum == target)
                    {
                        return (true, items[i], items[lo], items[hi]);
                    }
                    else if (sum > target)
                    {
                        hi -= 1;
                    }
                    else
                    {
                        var d = target - sum;
                        if (diff > d)
                        {
                            f = i;
                            s = lo;
                            t = hi;
                            diff = d;
                        }

                        lo += 1;
                    }
                }
            }

            return diff == int.MaxValue ? (false, null, null, null) : (true, items[f], items[s], items[t]);
        }

        private static List<Item> GetItems(string fileName, int target)
        {
            var items = new List<Item>();
            using (var file = new StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var item = Item.Parse(line);
                    if (item.Price >= target)
                    {
                        // Since the input file is sorted against the price, we
                        // only read the items that can actually be used for searching.
                        break;
                    }

                    items.Add(item);
                }
            }

            return items;
        }
    }
}

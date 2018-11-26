using System.Collections.Generic;
using System.Linq;
using Challenge2.Models;
using Xunit;

namespace Challenge2.Tests
{
    public class Tests
    {
        private static readonly List<Item> Items = new List<Item>
        {
            new Item {Name = "Candy Bar", Price = 500},
            new Item {Name = "Paperback Book", Price = 700},
            new Item {Name = "Detergent", Price = 1000},
            new Item {Name = "Headphones", Price = 1400},
            new Item {Name = "Earmuffs", Price = 2000},
            new Item {Name = "Bluetooth Stereo", Price = 6000},
        };

        [Fact]
        public void ShouldFindPairForExactMatch()
        {
            var target = 2500;
            var items = this.GetTestItems(target);

            var (found, first, second) = Program.FindPair(items, target);

            Assert.True(found);
            Assert.Equal("Candy Bar", first.Name);
            Assert.Equal(500, first.Price);
            Assert.Equal("Earmuffs", second.Name);
            Assert.Equal(2000, second.Price);
            Assert.Equal(target, first.Price + second.Price);
        }

        [Fact]
        public void ShouldFindPairForLowerMatch()
        {
            var target = 2300;
            var items = this.GetTestItems(target);

            var (found, first, second) = Program.FindPair(items, target);

            Assert.True(found);
            Assert.Equal("Paperback Book", first.Name);
            Assert.Equal(700, first.Price);
            Assert.Equal("Headphones", second.Name);
            Assert.Equal(1400, second.Price);
            Assert.True(first.Price + second.Price < target);
        }

        [Fact]
        public void ShouldNotFindPairIfNoLowerOrExactMatch()
        {
            var target = 1100;
            var items = this.GetTestItems(target);

            var (found, first, second) = Program.FindPair(items, target);

            Assert.False(found);
            Assert.Null(first);
            Assert.Null(second);
        }

        [Fact]
        public void ShouldFindTrioForExactMatch()
        {
            var target = 2900;
            var items = this.GetTestItems(target);

            var (found, one, two, three) = Program.FindTrio(items, target);

            Assert.True(found);
            Assert.Equal("Candy Bar", one.Name);
            Assert.Equal(500, one.Price);
            Assert.Equal("Detergent", two.Name);
            Assert.Equal(1000, two.Price);
            Assert.Equal("Headphones", three.Name);
            Assert.Equal(1400, three.Price);
            Assert.Equal(target, one.Price + two.Price + three.Price);
        }

        [Fact]
        public void ShouldFindTrioForLowerMatch()
        {
            var target = 4000;
            var items = this.GetTestItems(target);

            var (found, one, two, three) = Program.FindTrio(items, target);

            Assert.True(found);
            Assert.Equal("Candy Bar", one.Name);
            Assert.Equal(500, one.Price);
            Assert.Equal("Headphones", two.Name);
            Assert.Equal(1400, two.Price);
            Assert.Equal("Earmuffs", three.Name);
            Assert.Equal(2000, three.Price);
            Assert.True(one.Price + two.Price + three.Price < target);
        }

        [Fact]
        public void ShouldNotFindTrioIfNoLowerOrExactMatch()
        {
            var target = 2100;
            var items = this.GetTestItems(target);

            var (found, one, two, three) = Program.FindTrio(items, target);

            Assert.False(found);
            Assert.Null(one);
            Assert.Null(two);
            Assert.Null(three);
        }

        private List<Item> GetTestItems(int target)
        {
            return Items.TakeWhile(i => i.Price < target).ToList();
        }
    }
}

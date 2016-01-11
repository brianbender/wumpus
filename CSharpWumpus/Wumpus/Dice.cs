using System;

namespace Wumpus
{
    public class Dice
    {
        private readonly Random _random;

        public Dice()
        {
            _random = new Random();
        }

        public Dice(int seed)
        {
            _random = new Random(seed);
        }

        public int RollD20() {
            return _random.Next(20) + 1;
        }

        public int RollD3() {
            return _random.Next(3) + 1;
        }

        public int RollD4() {
            return _random.Next(4) + 1;
        }
    }
}
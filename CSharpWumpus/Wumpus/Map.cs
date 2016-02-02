using System.Collections.Generic;

namespace Wumpus
{
    public class Map
    {
        private readonly int[,] _exits =
        {
            {0, 0, 0},
            {2, 5, 8}, {1, 3, 10}, {2, 4, 12}, {3, 5, 14}, {1, 4, 6},
            {5, 7, 15}, {6, 8, 17}, {1, 7, 9}, {8, 10, 18}, {2, 9, 11},
            {10, 12, 19}, {3, 11, 13}, {12, 14, 20}, {4, 13, 15}, {6, 14, 16},
            {15, 17, 20}, {7, 16, 18}, {9, 17, 19}, {11, 18, 20}, {13, 16, 19}
        };

        public List<int> GetNeighboringRooms(int location)
        {
            var neighboringRooms = new List<int>();

            for (var i = 0; i < 3; i++)
            {
                neighboringRooms.Add(_exits[location, i]);
            }
            return neighboringRooms;
        }

        public int GetNeighboringRoom(int location, int direction)
        {
            return GetNeighboringRooms(location)[direction - 1];
        }

        public int GetRandomNeighboringRoom(int location, Dice dice)
        {
            return _exits[location, dice.RollD3() - 1];
        }

        public bool IsValidRoomToMoveTo(int roomPlayerIsIn, int moveTo)
        {
            var pathIndex = 0;
            var valid = false;
            do
            {
                if (_exits[roomPlayerIsIn, pathIndex] == moveTo ||
                    moveTo == roomPlayerIsIn)
                {
                    valid = true;
                }
                ++pathIndex;
            } while (pathIndex < 3);
            return valid;
        }

        public bool IsNeighboringRoom(int curRoom, int roomToFireTo)
        {
            for (var i = 0; i < 3; i++)
            {
                if (_exits[curRoom, i] == roomToFireTo)
                    return true;
            }
            return false;
        }
    }
}
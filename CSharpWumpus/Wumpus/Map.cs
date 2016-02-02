using System.Collections.Generic;

namespace Wumpus
{
    public class Map
    {
        public readonly int[,] exits =
        {
            {0, 0, 0, 0},
            {0, 2, 5, 8}, {0, 1, 3, 10}, {0, 2, 4, 12}, {0, 3, 5, 14}, {0, 1, 4, 6},
            {0, 5, 7, 15}, {0, 6, 8, 17}, {0, 1, 7, 9}, {0, 8, 10, 18}, {0, 2, 9, 11},
            {0, 10, 12, 19}, {0, 3, 11, 13}, {0, 12, 14, 20}, {0, 4, 13, 15}, {0, 6, 14, 16},
            {0, 15, 17, 20}, {0, 7, 16, 18}, {0, 9, 17, 19}, {0, 11, 18, 20}, {0, 13, 16, 19}
        };

        public List<int> GetNeighboringRooms(int location)
        {
            var neighboringRooms = new List<int>();

            for (var i = 1; i <= 3; i++)
            {
                neighboringRooms.Add(exits[location, i]);
            }
            return neighboringRooms;
        }

        public int GetRandomNeighboringRoom(int location, Dice dice)
        {
            return exits[location, dice.RollD3()];
        }

        public bool IsValidRoomToMoveTo(int roomPlayerIsIn, int moveTo)
        {
            int pathIndex = 1;
            var valid = false;
            do
            {
                if (exits[roomPlayerIsIn, pathIndex] == moveTo ||
                    moveTo == roomPlayerIsIn)
                {
                    valid = true;
                }
                ++pathIndex;
            } while (pathIndex <= 3);
            return valid;
        }
    }
}
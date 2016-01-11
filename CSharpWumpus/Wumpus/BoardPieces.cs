namespace Wumpus
{
    public class BoardPieces
    {
        private readonly int[] _lastPiecesGenerated = new int[7];
        public int[] _pieces = new int[7];

        public void GenerateBoardPieces(Dice dice)
        {
            CreateBoardPieces(dice);
            EnsureValidPieces(dice);
        }

        private void CreateBoardPieces(Dice dice)
        {
            var hazard = 1;
            var maxHazard = 6;
            do
            {
                _pieces[hazard] = dice.RollD20();
                _lastPiecesGenerated[hazard] = _pieces[hazard];
                ++hazard;
            } while (hazard <= maxHazard);
        }

        private void EnsureValidPieces(Dice dice)
        {
            for (var k = 1; k <= 6; ++k)
                for (var j = 1; j <= 6; ++j)
                    if (j != k && _pieces[j] == _pieces[k]) GenerateBoardPieces(dice);
        }

        public void ResetToLastPieces()
        {
            var j = 1;
            do
            {
                _pieces[j] = _lastPiecesGenerated[j];
                ++j;
            } while (j <= 6);
        }
    }
}
using System;

namespace Wumpus
{
    public class Game
    {
        private readonly BoardPieces _boardPieces;
        private readonly IO _io;

        private readonly Map _map = new Map();
        

        private int _actionTaken;
        private int[] _arrowFiringPath;
        private int _arrowsLeft;
        private int _currentLine;
        private int _gameOverStatus;

        private int _inputInteger;
        private char _istr;
        private int _ll;
        private int _nextLine;
        private int _pathIndex;

        public Game(IO io)
        {
            _io = io;
            EarlyExit = 1150;
            Dice = new Dice();
            _boardPieces = new BoardPieces();
        }


        public int EarlyExit { get; set; }

        public Dice Dice { get; set; }

        public int PathIndex
        {
            set { _pathIndex = value; }
            get { return _pathIndex; }
        }

        public void Play()
        {
            try
            {
                _currentLine = 5;
                _istr = '\0';


                _arrowFiringPath = new int[6];
                _arrowsLeft = 5;
                _ll = _arrowsLeft;
                _actionTaken = 1;
                _gameOverStatus = 0;

                _pathIndex = 0;
                _inputInteger = 0;
                _istr = GiveIntroduction(_istr);
                _boardPieces.GenerateBoardPieces(Dice);
                while (_currentLine <= 1150 && EarlyExit != _currentLine)
                {
                    _nextLine = _currentLine + 1;
                    switch (_currentLine)
                    {
                        case 230:
                            StartGame();
                            RunGameUntilGameOver();
                            HandleGameOver();
                            _boardPieces.ResetToLastPieces();
                            break; // 350 next j
                        case 355:
                            _io.Prompt("SAME SETUP (Y-N)");
                            _istr = _io.ReadChar();
                            if (_istr != 'Y' && _istr != 'y') { 
                                _boardPieces.GenerateBoardPieces(Dice);
                                _nextLine = 230;
                            }
                            break; // 365 if (i$ <> "Y") and (i$ <> "y") then 170
                        case 370:
                            _nextLine = 230;
                            break; // 370 goto 230
                    }
                    _currentLine = _nextLine;
                }
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block
                _io.WriteLine(e.StackTrace);
            }
        }

        private void StartGame()
        {
            _arrowsLeft = 5;
            _ll = _boardPieces.GetPlayerLocation();
            _io.WriteLine("HUNT THE WUMPUS");
        }

        private void RunGameUntilGameOver()
        {
            do
            {
                PrintRoomStatus();
                PromptShootOrMove();
                switch (_actionTaken)
                {
                    case 1:
                        ShootArrowAndMoveWumpus();
                        break;
                    case 2:
                        Move();
                        break;
                }
            } while (_gameOverStatus == 0);
        }

        private void Move()
        {
            _ll = GetValidRoom();
            var done = false;
            do
            {
                _boardPieces.SetPlayerLocation(_ll);
                if (_ll == _boardPieces._pieces[2])
                {
                    _io.WriteLine("... OOPS! BUMPED A WUMPUS!");
                    MoveWumpus();
                    if (_gameOverStatus != 0) return;
                    done = true;
                }
                else if (FellInPit())
                {
                    _io.WriteLine("YYYYIIIIEEEE . . . FELL IN PIT");
                    _gameOverStatus = -1;
                    return;
                }
                else if (HitABat())
                {
                    _io.WriteLine("ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!");
                    _ll = Dice.RollD20();
                }
                else
                {
                    done = true;
                }
            } while (!done);
        }

        private int GetValidRoom()
        {
            int moveTo;
            bool valid;
            do
            {
                moveTo = PromptMovement();
                var roomPlayerIsIn = _boardPieces.GetPlayerLocation();

                valid = _map.IsValidRoomToMoveTo(roomPlayerIsIn, moveTo);

                if (!valid)
                    _io.Prompt("NOT POSSIBLE - ");
                
            } while (!valid);
            return moveTo;
        }

        private int PromptMovement()
        {
            int ll;
            _gameOverStatus = 0;
            do
            {
                _io.Prompt("WHERE TO ");
                ll = _io.readInt();
            } while ((ll < 1) || (ll > 20));
            return ll;
        }

        private bool HitABat()
        {
            return _ll == _boardPieces._pieces[5] || _ll == _boardPieces._pieces[6];
        }

        private bool FellInPit()
        {
            return _ll == _boardPieces._pieces[3] || _ll == _boardPieces._pieces[4];
        }

        private void ShootArrowAndMoveWumpus()
        {
            PromptForArrowDistance();
            PromptForArrowPath();
            _ll = _boardPieces.GetPlayerLocation();
            _pathIndex = 1;
            do
            {
                if (!_map.IsNeighboringRoom(_ll, _arrowFiringPath[_pathIndex]))
                    _ll = _map.GetRandomNeighboringRoom(_ll, Dice);
                else
                    _ll = _arrowFiringPath[_pathIndex];
                if (YouShotTheWumpusWithAnArrow(_boardPieces, _arrowFiringPath[_pathIndex]))
                {
                    _io.WriteLine("AHA! YOU GOT THE WUMPUS!");
                    _gameOverStatus = 1;
                    return;
                }
                if (YouShotYourself())
                {
                    _io.WriteLine("OUCH! ARROW GOT YOU!");
                    _gameOverStatus = -1;
                    return;
                }
                ++_pathIndex;
            } while (_pathIndex <= _inputInteger);
            _io.WriteLine("MISSED");
            _ll = _boardPieces.GetPlayerLocation();
            MoveWumpus();
            _arrowsLeft = _arrowsLeft - 1;
            if (_arrowsLeft <= 0) _gameOverStatus = -1;
        }

        private bool YouShotYourself()
        {
            return _ll == _boardPieces.GetPlayerLocation();
        }

        private void PromptShootOrMove()
        {
            while (true)
            {
                _io.Prompt("SHOOT OR MOVE (S-M) ");
                _istr = _io.ReadChar();
                if (_istr == 'S' || _istr == 's')
                {
                    _actionTaken = 1;
                    break;
                }
                if (_istr == 'M' || _istr == 'm')
                {
                    _actionTaken = 2;
                    break;
                }
            }
        }

        private void PrintRoomStatus()
        {
            _io.WriteLine("");
            var neighboringRooms = _map.GetNeighboringRooms(_boardPieces.GetPlayerLocation());

            for (var j = 2; j <= 6; ++j)
                if (neighboringRooms.Contains(_boardPieces._pieces[j]))
                    PrintNearHazard(j);
            _io.Prompt("YOUR ARE IN ROOM ");
            _io.WriteLine(_boardPieces.GetPlayerLocation().ToString());
            _io.Prompt("TUNNELS LEAD TO ");
            _io.Prompt(neighboringRooms[0].ToString());
            _io.Prompt(" ");
            _io.Prompt(neighboringRooms[1].ToString());
            _io.Prompt(" ");
            _io.WriteLine(neighboringRooms[2].ToString());
            _io.WriteLine("");
        }

        private void HandleGameOver()
        {
            if (_gameOverStatus > 0) _io.WriteLine("HEE HEE HEE - THE WUMPUS'LL GET YOU NEXT TIME!!");
            else _io.WriteLine("HA HA HA - YOU LOSE!");
        }

        private void MoveWumpus()
        {
            _pathIndex = Dice.RollD4();
            if (_pathIndex != 4)
                _boardPieces._pieces[2] = _map.GetNeighboringRoom(_boardPieces._pieces[2], _pathIndex);
            if (_boardPieces._pieces[2] == _ll)
            {
                _io.WriteLine("TSK TSK TSK - WUMPUS GOT YOU!");
                _gameOverStatus = -1;
            }
        }

        private static bool YouShotTheWumpusWithAnArrow(BoardPieces boardPieces, int ll)
        {
            return ll == boardPieces._pieces[2];
        }

        private void PromptForArrowPath()
        {
            for (_pathIndex = 1; _pathIndex <= _inputInteger; ++_pathIndex)
            {
                do
                {
                    _io.Prompt("ROOM # ");
                    _arrowFiringPath[_pathIndex] = _io.readInt();
                    if (InvalidArrowPath())
                        _io.WriteLine("ARROWS AREN'T THAT CROOKED - TRY ANOTHER ROOM");
                } while (InvalidArrowPath());
            }
        }

        private bool InvalidArrowPath()
        {
            return _pathIndex > 2 && _arrowFiringPath[_pathIndex] == _arrowFiringPath[_pathIndex - 2];
        }

        private void PromptForArrowDistance()
        {
            _gameOverStatus = 0;
            do
            {
                _io.Prompt("NO. OF ROOMS (1-5) ");
                _inputInteger = _io.readInt();
            } while (_inputInteger > 5 || _inputInteger < 1);
        }

        private void PrintNearHazard(int j)
        {
            switch (j - 1)
            {
                case 1:
                    NearAWumpus();
                    break;
                case 2:
                case 3:
                    NearAPit();
                    break;
                case 4:
                case 5:
                    NearBats();
                    break;
            }
        }

        private void NearBats()
        {
            _io.WriteLine("BATS NEARBY!");
        }

        private void NearAPit()
        {
            _io.WriteLine("I FEEL A DRAFT");
        }

        private void NearAWumpus()
        {
            _io.WriteLine("I SMELL A WUMPUS!");
        }

        private char GiveIntroduction(char istr)
        {
            _io.Prompt("INSTRUCTIONS (Y-N) ");
            istr = _io.ReadChar();
            if (!(istr == 'N' || istr == 'n'))
                _io.GiveInstructions();
            return istr;
        }
    }
}
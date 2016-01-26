using System;
using System.Collections.Generic;

namespace Wumpus
{
    public class Game
    {
        private readonly BoardPieces _boardPieces;
        private readonly IO _io;

        private readonly int[,] exits =
        {
            {0, 0, 0, 0},
            {0, 2, 5, 8}, {0, 1, 3, 10}, {0, 2, 4, 12}, {0, 3, 5, 14}, {0, 1, 4, 6},
            {0, 5, 7, 15}, {0, 6, 8, 17}, {0, 1, 7, 9}, {0, 8, 10, 18}, {0, 2, 9, 11},
            {0, 10, 12, 19}, {0, 3, 11, 13}, {0, 12, 14, 20}, {0, 4, 13, 15}, {0, 6, 14, 16},
            {0, 15, 17, 20}, {0, 7, 16, 18}, {0, 9, 17, 19}, {0, 11, 18, 20}, {0, 13, 16, 19}
        };

        private readonly Stack<int> ReturnLine = new Stack<int>();
        private int[] _arrowFiringPath;
        private int _arrowsLeft;
        private int _currentLine;
        private int _gameOverStatus;

        private int _inputInteger;
        private char _istr;
        private int _k1;
        private int _ll;
        private int _nextLine;
        private int ActionTaken;
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

//TODO remove after refactoring so that this isn't needed by tests

        public void Play()
        {
            try
            {
                _currentLine = 5;
                _istr = '\0';


                _arrowFiringPath = new int[6];
                _arrowsLeft = 5;
                _ll = _arrowsLeft;
                ActionTaken = 1;
                _gameOverStatus = 0;

                _pathIndex = 0;
                _k1 = 0;
                _inputInteger = 0;
                while (_currentLine <= 1150 && EarlyExit != _currentLine)
                {
                    _nextLine = _currentLine + 1;
                    switch (_currentLine)
                    {
                        case 15:
                            _istr = GiveIntroduction(_istr);
                            break; // 25 if (i$ = "N") or (i$ = "n") then 35
                        case 170:
                            _boardPieces.GenerateBoardPieces(Dice);
                            break;
                        case 230:
                            _arrowsLeft = 5;
                            _ll = _boardPieces._pieces[1];
                            _io.WriteLine("HUNT THE WUMPUS");
                            break; 
                        case 255:
                            PrintRoomStatus();
                            break; 
                        case 265:
                            PromptShootOrMove();
                            break; 
                        case 270:
                            switch (ActionTaken)
                            {
                                case 1:
                                    _nextLine = 280;
                                    break;
                                case 2:
                                    _nextLine = 300;
                                    break;
                            }
                            break; // 270 on o goto 280,300
                        case 280:
                            gosub(715, 285);
                            break; // 280 gosub 715
                        case 285:
                            if (_gameOverStatus == 0) _nextLine = 255;
                            break; // 285 if f = 0 then 255
                        case 290:
                            _nextLine = 310;
                            break; // 290 goto 310
                        case 300:
                            gosub(975, 305);
                            break; // 300 gosub 975
                        case 305:
                            if (_gameOverStatus == 0) _nextLine = 255;
                            break; // 305 if f = 0 then 255
                        case 310:
                            HandleGameOver();
                            _boardPieces.ResetToLastPieces();
                            break; // 350 next j
                        case 355:
                            _io.Prompt("SAME SETUP (Y-N)");
                            _istr = _io.ReadChar();
                            if (_istr != 'Y' && _istr != 'y') _nextLine = 170;
                            break; // 365 if (i$ <> "Y") and (i$ <> "y") then 170
                        case 370:
                            _nextLine = 230;
                            break; // 370 goto 230
                        case 590:
                            PrintRoomStatus();
                            break; // 660 print
                        case 665:
                            returnFromGosub();
                            break; // 665 return
                        case 675:
                            PromptShootOrMove();
                            returnFromGosub();
                            break; // 710 return
                        case 720:
                            PromptForArrowDistance();
                            PromptForArrowPath();
                            _ll = _boardPieces._pieces[1];
                            _pathIndex = 1;
                            break; // 805 for k = 1 to j9
                        case 810:
                            if (CanArrowGoToNextRoom())
                                _nextLine = 895;
                            else
                            {
                                _ll = exits[_ll, Dice.RollD3()];
                                _nextLine = 900;
                            }
                            break; // 835 goto 900
                        case 840:
                            ++_pathIndex;
                            if (_pathIndex <= _inputInteger) _nextLine = 810;
                            break; // 840 next k
                        case 845:
                            _io.WriteLine("MISSED");
                            _ll = _boardPieces._pieces[1];
                            gosub(935, 865);
                            break; // 860 gosub 935
                        case 870:
                            _arrowsLeft = _arrowsLeft - 1;
                            if (_arrowsLeft > 0) returnFromGosub();
                            break; // 875 if a > 0 then 885
                        case 880:
                            _gameOverStatus = -1;
                            returnFromGosub();
                            break; // 885 return
                        case 895:
                            _ll = _arrowFiringPath[_pathIndex];
                            if (YouShotTheWumpusWithAnArrow(_boardPieces, _arrowFiringPath[_pathIndex]))
                            {
                                _io.WriteLine("AHA! YOU GOT THE WUMPUS!");
                                _gameOverStatus = 1;
                                returnFromGosub();
                            }
                            break;
                        case 920:
                            if (_ll != _boardPieces._pieces[1]) _nextLine = 840;
                            else
                            {
                                _io.WriteLine("OUCH! ARROW GOT YOU!");
                                _nextLine = 880;
                            }
                            break; // 930 goto 880
                        case 940:
                            // Wumpus movement
                            MoveWumpus();
                            returnFromGosub();
                            break; // 970 return
                        case 980:
                            _gameOverStatus = 0;
                            break; // 980 f = 0
                        case 985:
                            do
                            {
                                _io.Prompt("WHERE TO ");
                                _ll = _io.readInt();
                            } while ((_ll < 1) || (_ll > 20));
                            break;
                        case 1005:
                            _pathIndex = 1;
                            do
                            {
                                if (exits[_boardPieces._pieces[1], _pathIndex] == _ll)
                                {
                                    _nextLine = 1045;
                                    break;
                                }
                                ++_pathIndex;
                            } while (_pathIndex <= 3);
                            if (_ll == _boardPieces._pieces[1]) _nextLine = 1045;
                            break; // 1025 if l = l(1) then 1045
                        case 1030:
                            _io.Prompt("NOT POSSIBLE - ");
                            _nextLine = 985;
                            break; // 1035 goto 985
                        case 1045:
                            _boardPieces._pieces[1] = _ll;
                            if (_ll != _boardPieces._pieces[2]) _nextLine = 1090;
                            break; // 1055 if l <> l(2) then 1090
                        case 1060:
                            _io.WriteLine("... OOPS! BUMPED A WUMPUS!");
                            gosub(940, 1075);
                            break; // 1070 gosub 940
                        case 1075:
                            if (_gameOverStatus == 0) _nextLine = 1090;
                            break; // 1075 if f = 0 then 1090
                        case 1080:
                            returnFromGosub();
                            break; // 1080 return
                        case 1090:
                            if (_ll == _boardPieces._pieces[3]) _nextLine = 1100;
                            break; // 1090 if l = l(3) then 1100
                        case 1095:
                            if (_ll != _boardPieces._pieces[4]) _nextLine = 1120;
                            break; // 1095 if l <> l(4) then 1120
                        case 1100:
                            _io.WriteLine("YYYYIIIIEEEE . . . FELL IN PIT");
                            _gameOverStatus = -1;
                            returnFromGosub();
                            break; // 1110 return
                        case 1120:
                            if (_ll == _boardPieces._pieces[5]) _nextLine = 1130;
                            break; // 1120 if l = l(5) then 1130
                        case 1125:
                            if (_ll != _boardPieces._pieces[6]) _nextLine = 1145;
                            break; // 1125 if l <> l(6) then 1145
                        case 1130:
                            _io.WriteLine("ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!");
                            _ll = Dice.RollD20();
                            _nextLine = 1045;
                            break; // 1140 goto 1045
                        case 1145:
                            returnFromGosub();
                            break; // 1145 return
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

        private void PromptShootOrMove()
        {
            while (true)
            {
                _io.Prompt("SHOOT OR MOVE (S-M) ");
                _istr = _io.ReadChar();
                if (_istr == 'S' || _istr == 's')
                {
                    ActionTaken = 1;
                    break;
                }
                if (_istr == 'M' || _istr == 'm')
                {
                    ActionTaken = 2;
                    break;
                }
            }
        }

        private void PrintRoomStatus()
        {
            _io.WriteLine("");
            var neighboringRooms = GetNeighboringRooms();

            for (var j = 2; j <= 6; ++j)
                if (neighboringRooms.Contains(_boardPieces._pieces[j]))
                    PrintNearHazard(j);
            _io.Prompt("YOUR ARE IN ROOM ");
            _io.WriteLine(_boardPieces._pieces[1].ToString());
            _io.Prompt("TUNNELS LEAD TO ");
            _io.Prompt(exits[_ll, 1].ToString());
            _io.Prompt(" ");
            _io.Prompt(exits[_ll, 2].ToString());
            _io.Prompt(" ");
            _io.WriteLine(exits[_ll, 3].ToString());
            _io.WriteLine("");
        }

        private void HandleGameOver()
        {
            if (_gameOverStatus > 0) _io.WriteLine("HEE HEE HEE - THE WUMPUS'LL GET YOU NEXT TIME!!");
            else _io.WriteLine("HA HA HA - YOU LOSE!");
        }

        private bool CanArrowGoToNextRoom()
        {
            for (_k1 = 1; _k1 <= 3; _k1++)
                if (exits[_ll, _k1] == _arrowFiringPath[_pathIndex])
                    return true;
            return false;
        }

        private void MoveWumpus()
        {
            _pathIndex = Dice.RollD4();
            if (_pathIndex != 4)
                _boardPieces._pieces[2] = exits[_boardPieces._pieces[2], _pathIndex];
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

        private List<int> GetNeighboringRooms()
        {
            var neighboringRooms = new List<int>();
            for (var i = 1; i <= 3; i++)
                neighboringRooms.Add(exits[GetPlayerLocation(), i]);
            return neighboringRooms;
        }

        private int GetPlayerLocation()
        {
            return _boardPieces._pieces[1];
        }

        private void PrintNearHazard(int j)
        {
            switch (j - 1)
            {
                // 610 on j-1 goto 615,625,625,635,635
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

        private void gosub(int gosubLine, int lineToReturnTo)
        {
            _nextLine = gosubLine;
            ReturnLine.Push(lineToReturnTo);
        }

        private void returnFromGosub()
        {
            if (ReturnLine.Count == 0)
                _nextLine = 1151;
            else
                _nextLine = ReturnLine.Pop();
        }
    }
}